# Action Buttons

Use actions to implement the logic and the PCF to render these actions as buttons on subgrids.

### Features
- Implement different buttons in subgrids of the same entity.
- Define wich actions will be render as buttons, the icons and tool tips.
- Request Input Parameters: Boolean, DateTime, Decimal, Float, Integer, Money, OptionSet, String and EntityReference.
- Users can select attributes of the host entity as input parameters to execute the action.
- Reduce to customizations on entity Ribbon

### Limitations
- Generic activity subgrids, not supported.
- Actions with Entity or EntityCollection parameters, not supported.
- Return values, not supported.
- EntityReference and OptionSet parameters only can be select using the host entity attributes.
- The same control can't be added in the same form.
- Only visible attributes with value (not null) can be selected as a suggestion.

### Enable To
- SingleLine.Text

### Prerequisites
- The action must be active and associated with an entity
- The component needs the SubgridName and JSON Config.

### Ready to use
The [managed]() solution is ideal for non developers. Import and use.

### Developers
The PCF call a Action (process) to obtain informations about the Actions related of subgrid entity type, then render the return as buttons.