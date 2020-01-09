import {IInputs, IOutputs} from "./generated/ManifestTypes";
import { ActionResponse } from "./models/ActionResponse";
import { Action } from "./models/Action";
import { ActionContract } from "./models/ActionContract";
import { ActionButtonConfig } from "./models/ActionButtonConfig";
import { InputParameter } from "./models/InputParameter";

export class ActionButtons implements ComponentFramework.StandardControl<IInputs, IOutputs> {

	private _container: HTMLDivElement;

	private _attributes : Xrm.Attributes.Attribute[];

	private _subgridEntityName : string;
	private _subgridName : string;
	private _subgrid : Xrm.Controls.GridControl;
	private _selectedRecords : Xrm.Collection.ItemCollection<Xrm.Controls.Grid.GridRow> | null;

	private _config : ActionButtonConfig[];
	private _actions : Action[];

	private _transactionalModal : HTMLDivElement;

	/**
	 * Empty constructor.
	 */
	constructor()
	{
		// this._actions = new Array();

		// let actionConfig : ActionButtonConfig;
		// actionConfig = new ActionButtonConfig();
		// actionConfig.ActionUniqueName = "try_Try";
		// actionConfig.WebResourceImage = "/WebResources/vnb_img";

		// this._config.push(actionConfig);
	}

	/**
	 * Used to initialize the control instance. Controls can kick off remote server calls and other initialization actions here.
	 * Data-set values are not initialized here, use updateView.
	 * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to property names defined in the manifest, as well as utility functions.
	 * @param notifyOutputChanged A callback method to alert the framework that the control has new outputs ready to be retrieved asynchronously.
	 * @param state A piece of data that persists in one session for a single user. Can be set at any point in a controls life cycle by calling 'setControlState' in the Mode interface.
	 * @param container If a control is marked control-type='standard', it will receive an empty div element within which it can render its content.
	 */
	public init(context: ComponentFramework.Context<IInputs>, notifyOutputChanged: () => void, state: ComponentFramework.Dictionary, container:HTMLDivElement)
	{
		this._container = document.createElement("div");
		container.append(this._container); //Main

		this._transactionalModal = document.createElement("div");
		container.append(this._transactionalModal); //Modal
	}

	/**
	 * Called when any value in the property bag has changed. This includes field values, data-sets, global values such as container height and width, offline status, control metadata values such as label, visible, etc.
	 * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to names defined in the manifest, as well as utility functions
	 */
	public updateView(context: ComponentFramework.Context<IInputs>): void
	{
		if(context.parameters.jsonConfig.raw)
		{
			if(Xrm.Page.getControl(this._subgridName) !== undefined)
			{
				this._config = new Array();
				this._config = JSON.parse(context.parameters.jsonConfig.raw!.toString());
				this._actions = new Array();
				this.CloseModal(false);
				this.Destroy();
				this._subgridName = context.parameters.subgridname.raw!.toString();
				this._subgrid = Xrm.Page.getControl<Xrm.Controls.GridControl>(this._subgridName);
				this._subgridEntityName = this._subgrid.getEntityName();
				this.RetrieveActions();
			}
		}
	}

	/** 
	 * It is called by the framework prior to a control receiving new data. 
	 * @returns an object based on nomenclature defined in manifest, expecting object[s] for property marked as “bound” or “output”
	 */
	public getOutputs(): IOutputs
	{
		return {};
	}

	/** 
	 * Called when the control is to be removed from the DOM tree. Controls should use this call for cleanup.
	 * i.e. cancelling any pending remote calls, removing listeners, etc.
	 */
	public destroy(): void {}

	/**
	 * Get SuBgrid Selected Rows and generate the Modal with InputParameters
	 * @param actionUniqueName Action Unique Name
	 */
	public Select(actionUniqueName : string) : void
	{
		this._attributes = Xrm.Page.getAttribute().filter(f => f.getValue() != undefined);
		this._selectedRecords = this._subgrid.getGrid().getSelectedRows();
		if(this._selectedRecords.getLength() > 0)
		{
			let action = this.RetrieveAction(actionUniqueName);
			if(action !== undefined)
				this.GenerateActionModal(action as Action);
		}
	}

	/**
	 * Get selected attribute value and define as InputParameter
	 * @param inputparameterName Attribute Option
	 */
	public SelectAttribute(inputparameterName : string) : void
	{
		let select = document.getElementById(inputparameterName) as HTMLSelectElement;
		if(select !== undefined)
		{
			let inputParameter = document.getElementById(inputparameterName.replace("select_attribute_", "")) as HTMLInputElement;
			let attribute = Xrm.Page.getAttribute(select.value);

			if(attribute)
			{
				switch(typeof(attribute.getValue()))
				{
					case "object":
						//Lookup
						if(attribute.getValue().length == 1)
						{
							let lookup = attribute.getValue()[0] as Xrm.LookupValue;
							inputParameter.value = lookup.name!.toString();
							inputParameter.setAttribute("logicalName", lookup.entityType);
							inputParameter.setAttribute("entityId", lookup.id);
						}
						else
						{
							if(attribute.getValue() instanceof Date)
							{
								let datetime = attribute.getValue() as Date;
								inputParameter.value = this.toDateString(datetime);
							}
						}
						break;

					case "boolean":
						inputParameter.checked = attribute.getValue();
						break;

					default:
						inputParameter.value = attribute.getValue().toString();
						break;
				}
			}
			else
				inputParameter.value = "";
		}
	}

	/**
	 * Run Action for Selected Records
	 * @param actionUniqueName Action Unique Name
	 */
	public RunAction(actionUniqueName : string) : void
	{
		const self = this;
		let requests : number = this._selectedRecords!.getLength();
		let responses : number = 0;
		let errors : string = "";
		this._selectedRecords!.forEach(selectRecord_ => {
			
			let request : any = this.GenerateRequest(selectRecord_, actionUniqueName);
			if(request)
			{
				Xrm.WebApi.online.execute(request).then(
					function (result) {
						if (result.ok) {
							responses++;
							if(responses == requests)
								self.CloseModal(true);
						}
					},
					function (error) {
						responses++;
						errors += error.innerror.message + "\r\n";
						if(responses == requests)
							alert(errors);
					}
				);
			}
		});
	}

	/**
	 * Clear the main container
	 */
	private Destroy()
	{
		while (this._container.lastChild) {
			this._container.removeChild(this._container.lastChild);
		}
	}

	/**
	 * Close Action Modal and clear selected rows
	 */
	public CloseModal(refreshSubGrid : boolean) : void
	{
		this._selectedRecords = null;
		while (this._transactionalModal.lastChild) {
			this._transactionalModal.removeChild(this._transactionalModal.lastChild);
		}

		if(refreshSubGrid)
			this._subgrid.refresh();
	}

	//TRANSFORMATIONS ---------------------------------------------------------------------------------------------------------------
	private toDateString(date: Date): string {
		return (date.getFullYear().toString() + '-' 
		   + ("0" + (date.getMonth() + 1)).slice(-2) + '-' 
		   + ("0" + (date.getDate())).slice(-2))
		   + 'T' + date.toTimeString().slice(0,5);
	}

	//ACTIONS -----------------------------------------------------------------------------------------------------------------------
	/**
	 * Get Target Record and define as a Target Parameter
	 * @param selectRecord Selected Record
	 */
	private GetTarget(selectRecord : Xrm.Controls.Grid.GridRow) : any
	{
		var entity : any;
		entity = new Object();
		entity.id = selectRecord.getData().getEntity().getId();
		entity.entityType = selectRecord.getData().getEntity().getEntityName();
		return entity;
	}
	
	/**
	 * Create the Action Request
	 * @param selectRecord Selected Record
	 * @param actionUniqueName Action Unique Name
	 */
	private GenerateRequest(selectRecord : Xrm.Controls.Grid.GridRow, actionUniqueName : string) : any
	{
		var request : any = new Object();
		var parameterTypes : any = new Object();

		//Target
		request.entity = this.GetTarget(selectRecord);
		parameterTypes.entity = {
			"typeName": "mscrm." + selectRecord.getData().getEntity().getEntityName(),
			"structuralProperty": 5
		};

		//Parameters
		let action = this._actions.filter(f=>f.UniqueName == actionUniqueName)[0];
		action.InputParameters.forEach(inputParameter => {

			let input = document.getElementById(inputParameter.Name) as HTMLInputElement;
			if(input && input.id)
			{
				let cds_type = input?.getAttribute("cds-type");
				if(cds_type !== undefined)
				{
					switch(cds_type)
					{
						case "string":
							if(input.value)
							{
								request[input.id] = input?.value;
								parameterTypes[input.id] = {
									"typeName": "Edm.String",
									"structuralProperty": 1
								};
							}
							break;
						case "boolean":
							if(input.checked)
							{
								request[input.id] = input?.checked;
								parameterTypes[input.id] = {
									"typeName": "Edm.Boolean",
									"structuralProperty": 1
								};
							}
							break;
						case "datetime":
							if(input.value)
							{
								request[input.id] = JSON.stringify(new Date(input?.value).toISOString());
								parameterTypes[input.id] = {
									"typeName": "Edm.DateTimeOffset",
									"structuralProperty": 1
								};
							}
						break;
						case "decimal":
						case "money":
							if(input.value)
							{
								request[input.id] = Number(input?.value);
								parameterTypes[input.id] = {
									"typeName": "Edm.Decimal",
									"structuralProperty": 1
								};
							}
						break;
						case "double":
							if(input.value)
							{
								request[input.id] = Number(input?.value);
								parameterTypes[input.id] = {
									"typeName": "Edm.Double",
									"structuralProperty": 1
								};
							}
						break;
						case "integer":
						case "optionset":
							if(input.value)
							{
								request[input.id] = Number(input?.value);
								parameterTypes[input.id] = {
									"typeName": "Edm.Int32",
									"structuralProperty": 1
								};
							}
						break;
						case "lookup":
							if(input.value)
							{
								let entityIdProperty : string | null = input.getAttribute("entityid");
								let entityLogicalNameProperty : string | null = input.getAttribute("logicalname");
								if(entityIdProperty && entityLogicalNameProperty)
								{
									var entityReference : any;
									entityReference = new Object();
									entityReference[entityLogicalNameProperty + "id"] = entityIdProperty;
									entityReference["@odata.type"] = "Microsoft.Dynamics.CRM." + entityLogicalNameProperty;
									request[input.id] = entityReference;

									parameterTypes[input.id] = {
										"typeName": "mscrm." + entityLogicalNameProperty,
										"structuralProperty": 5
									};
								}
							}
							break;
					}
				}
			}
		});
	
		if(this.VerifyRequiredParameters(actionUniqueName, parameterTypes))
		{
			//Request
			request.getMetadata = function () {
				return {
					boundParameter : "entity",
					parameterTypes : parameterTypes,
					operationType: 0,
					operationName: actionUniqueName
				}
			};
		}
		else
			request = null;

		return request;
	}

	/**
	* Verify if all required parameters has value
	*/
	private VerifyRequiredParameters(actionUniqueName : string, parameterTypes : any) : boolean
	{
		let requiredExceptions : number = 0;
		let action = this.RetrieveAction(actionUniqueName);
		action?.InputParameters.forEach(inputParameter => {
			let value = parameterTypes[inputParameter.Name];
			if(inputParameter.Name != "Target" && inputParameter.Required && !value) { requiredExceptions++ };
		});

		return requiredExceptions == 0 ? true : false;
	}
	
	/**
	 * Retrieve Action Config reference
	 * @param actionUniqueName Action Unique Name
	 */
	private RetrieveActionConfig(actionUniqueName : string) : ActionButtonConfig | null
	{
		let actionButtonConfig = this._config.filter(f => f.ActionUniqueName == actionUniqueName)[0];
		if(actionButtonConfig !== undefined)
			return actionButtonConfig;
		else
			return null;
	}

	/**
	* Retrieve Action reference
	* @param actionUniqueName Action Unique Name
	*/
	private RetrieveAction(actionUniqueName : string) : Action | null
	{
		let action = this._actions.filter(f => f.UniqueName == actionUniqueName)[0];
		if(action !== undefined)
			return action;
		else
			return null;
	}

	/**
	 * Retrieve All Actions by Entity SubGrid
	 */
	private RetrieveActions() : void
	{
		let request = new ActionContract();
		request.PrimaryEntity = this._subgridEntityName;
		request.getMetadata = function() {
			return {
				boundParameter: null,
				parameterTypes: {
					"PrimaryEntity": {
						"typeName": "Edm.String",
						"structuralProperty": 1
					}
				},
				operationType: 0,
				operationName: "vnb_RetrieveActions"
			};
		}

		const self = this;
		Xrm.WebApi.online.execute(request).then(
			function (result) {
				if (result.ok) {
					self.FetchStream(self, result.body);
				}
			},
			function (error) {
				Xrm.Utility.alertDialog(error.message, function(){});
			}
		);
	}

	private FetchStream(caller : ActionButtons, stream : ReadableStream | null) : void {
		const reader = stream!.getReader();
		let text : string;
		text = "";
		reader.read().then(function processText({ done, value }) : void {  
			
			if(done)
			{
				let content: ActionResponse = JSON.parse(text);
				let retrieveActionsResponseModel : any;
				retrieveActionsResponseModel = new Object();
				retrieveActionsResponseModel.Actions = new Array();
				retrieveActionsResponseModel = JSON.parse(content.RetrieveActionsResponseModel);

				if(retrieveActionsResponseModel !== undefined)
				{
					for (let index = 0; index < retrieveActionsResponseModel.Actions.length; index++) {
						const element = retrieveActionsResponseModel.Actions[index];
						caller._actions.push(element);
						caller.GenerateActionButton(element);
					}
				}
				return;
			}
			
			if(value)
				text += new TextDecoder("utf-8").decode(value);
				reader.read().then(processText);
		});
	}

	//HTML --------------------------------------------------------------------------------------------------------------------------
	/**
	* Render HTMLButtonElement representing the Action
	*/
	public GenerateActionButton(action : Action)
	{
		let actionButtonConfig = this.RetrieveActionConfig(action.UniqueName);

		let actionButton : HTMLButtonElement;
		actionButton = document.createElement("button");
		actionButton.id = action.UniqueName;
		actionButton.title = actionButtonConfig!.Tooltip;
		actionButton.setAttribute("class", "buttonAction");
		actionButton.addEventListener("click", this.Select.bind(this, actionButton.id));
		{
			if(actionButtonConfig !== undefined)
			{
				let img: HTMLImageElement;
				img = document.createElement("img");
				img.setAttribute("class", "img");
				img.setAttribute("src", Xrm.Page.context.getClientUrl() + actionButtonConfig?.WebResourceImage);
				actionButton.append(img);
			}
			else
			{
				actionButton.textContent = action.UniqueName + " not found";
			}
		}
		this._container.append(actionButton);
	}

	/**
	* Render HTMLDialogElement representing the Action
	* @param action Action Reference
	*/
	private GenerateActionModal(action : Action) : void
	{
		let actionModal : HTMLDialogElement;
		actionModal = document.createElement("dialog");
		actionModal.id = action.UniqueName;
		actionModal.addEventListener("close", this.CloseModal.bind(this, true));
		actionModal.append(this.GenerateActionHeader());
		action.InputParameters.forEach(inputParameter => { actionModal.append(this.GenerateInputParameter(inputParameter));});
		actionModal.append(this.GenerateActionModalButtons(action));
		this._transactionalModal.append(actionModal);
		actionModal.showModal();
	}

	/**
	* Render HTMLDivElement representing selected rows
	*/
	private GenerateActionHeader() : HTMLDivElement
	{
		let divHeader : HTMLDivElement;
		divHeader = document.createElement("div");

		let ul : HTMLUListElement;
		ul = document.createElement("ul");
		ul.setAttribute("class", "ul");
		this._selectedRecords!.forEach(selectedRecord => {
			let li : HTMLLIElement;
			li = document.createElement("li");
			li.setAttribute("class", "line");
			li.textContent = selectedRecord.getData().getEntity().getPrimaryAttributeValue();
			ul.append(li);
		});

		divHeader.append(ul);
		return divHeader;
	} 

	/**
	 * Render Inputs and Host Form Attribute Options
	 * @param inputparameter Action InputParameter
	 */
	private GenerateInputParameter(inputparameter : InputParameter) : HTMLDivElement
	{
		let divInputParameters : HTMLDivElement;
		divInputParameters = document.createElement("div");
		divInputParameters.setAttribute("class","input_container");

		if(inputparameter.Name !== "Target")
		{
			let attributeType : string = "";

			//Label
			let divAttributeLabel : HTMLDivElement;
			divAttributeLabel = document.createElement("div");
			{
				let inputRequired : HTMLLabelElement;
				inputRequired = document.createElement("label");
				inputRequired.setAttribute("class","required");
				inputRequired.textContent = inputparameter.Required ? "*" : " ";
				divAttributeLabel.append(inputRequired);

				let inputLabel : HTMLLabelElement;
				inputLabel = document.createElement("label");
				inputLabel.textContent = inputparameter.Name;
				divAttributeLabel.append(inputLabel);
			}
			divInputParameters.append(divAttributeLabel);

			//Attribute
			let divAttributeValue : HTMLDivElement;
			divAttributeValue = document.createElement("div");
			{
				let inputContent : HTMLElement;
				inputContent = document.createElement("input");
				inputContent.id = inputparameter.Name;
				switch(inputparameter.Type)
				{
					case "InArgument(x:String)":
						attributeType = "string";
						inputContent.setAttribute("type","text");
					break;

					case "InArgument(x:Boolean)":
						attributeType = "boolean";
						inputContent.setAttribute("type","checkbox");
					break;

					case "InArgument(s:DateTime)":
						attributeType = "datetime";
						inputContent.setAttribute("type","datetime-local");
					break;

					case "InArgument(x:Decimal)":
						attributeType = "decimal";
						inputContent.setAttribute("type","number");
					break;

					case "InArgument(x:Double)":
						attributeType = "double";
						inputContent.setAttribute("type","number");
					break;

					case "InArgument(mxs:Money)":
						attributeType = "money";
						inputContent.setAttribute("type","number");
					break;

					case "InArgument(x:Int32)":
						attributeType = "integer";
						inputContent.setAttribute("type","number");
					break;

					case "InArgument(mxs:EntityReference)":
						attributeType = "lookup";
						inputContent.setAttribute("type","text");
						inputContent.setAttribute("disabled", "true");
						//to-do...
						break;

					case "InArgument(mxs:OptionSetValue)":
						attributeType = "optionset";
						inputContent.setAttribute("type","text");
						inputContent.setAttribute("disabled", "true");
						//to-do...
					break;
				}

				inputContent.setAttribute("cds-type", attributeType);
				divAttributeValue.append(inputContent);
			}
			divInputParameters.append(divAttributeValue);

			//Sugestion
			let divAttributeSugestion : HTMLDivElement;
			divAttributeSugestion = document.createElement("div");
			{
				let selectAttribute : HTMLSelectElement;
				selectAttribute = document.createElement("select");
				selectAttribute.setAttribute("class", "select");
				selectAttribute.id = "select_attribute_" + inputparameter.Name;
				selectAttribute.disabled = this._attributes.filter(f => f.getAttributeType() == attributeType).length > 0 ? false : true;
				selectAttribute.addEventListener("change", this.SelectAttribute.bind(this, selectAttribute.id));

				let nullOption : HTMLOptionElement;
				nullOption = document.createElement("option");
				nullOption.value = "null";
				nullOption.textContent = "-";
				selectAttribute.append(nullOption);

				let typedAttributes = this._attributes.filter(f => f.getAttributeType() == attributeType);

				typedAttributes.forEach(attribute => {
					
					let currentControl = Xrm.Page.getControl(attribute.getName());
					if(currentControl && currentControl.getVisible() == true)
					{
						let option : HTMLOptionElement;
						option = document.createElement("option");
						option.value = attribute.getName();
						option.textContent = currentControl.getLabel();
						selectAttribute.append(option);
					}
				});

				divAttributeSugestion.append(selectAttribute);
			}
			
			divInputParameters.append(divAttributeSugestion);
		}

		return divInputParameters;
	}

	/**
	 * Render Run and Cancel Button
	 * @param action 
	 */
	private GenerateActionModalButtons(action : Action) : HTMLDivElement
	{
		let divActionButtons : HTMLDivElement;
		divActionButtons = document.createElement("div");

		//Run
		let buttonOK : HTMLButtonElement;
		buttonOK = document.createElement("button");
		buttonOK.setAttribute("class","buttonRun");
		buttonOK.id = action.UniqueName;
		buttonOK.textContent = "Run";
		buttonOK.addEventListener("click", this.RunAction.bind(this, buttonOK.id));
		divActionButtons.append(buttonOK);

		//Cancel
		let buttonCancel : HTMLButtonElement;
		buttonCancel = document.createElement("button");
		buttonCancel.setAttribute("class","buttonCancel");
		buttonCancel.textContent = "Cancel";
		buttonCancel.addEventListener("click", this.CloseModal.bind(this, true));
		divActionButtons.append(buttonCancel);

		return divActionButtons;
	}
}