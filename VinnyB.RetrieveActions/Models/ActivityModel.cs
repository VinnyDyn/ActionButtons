using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveActions.Models
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netfx/2009/xaml/activities")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netfx/2009/xaml/activities", IsNullable = false)]
    public partial class Activity
    {

        private MembersProperty[] membersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://schemas.microsoft.com/winfx/2006/xaml")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Property", IsNullable = false)]
        public MembersProperty[] Members
        {
            get
            {
                return this.membersField;
            }
            set
            {
                this.membersField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/winfx/2006/xaml")]
    public partial class MembersProperty
    {

        private MembersPropertyPropertyAttributes propertyAttributesField;

        private string nameField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property.Attributes")]
        public MembersPropertyPropertyAttributes PropertyAttributes
        {
            get
            {
                return this.propertyAttributesField;
            }
            set
            {
                this.propertyAttributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/winfx/2006/xaml")]
    public partial class MembersPropertyPropertyAttributes
    {

        private ArgumentRequiredAttribute argumentRequiredAttributeField;

        private ArgumentTargetAttribute argumentTargetAttributeField;

        private ArgumentDescriptionAttribute argumentDescriptionAttributeField;

        private ArgumentDirectionAttribute argumentDirectionAttributeField;

        private ArgumentEntityAttribute argumentEntityAttributeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
            "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
        public ArgumentRequiredAttribute ArgumentRequiredAttribute
        {
            get
            {
                return this.argumentRequiredAttributeField;
            }
            set
            {
                this.argumentRequiredAttributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
            "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
        public ArgumentTargetAttribute ArgumentTargetAttribute
        {
            get
            {
                return this.argumentTargetAttributeField;
            }
            set
            {
                this.argumentTargetAttributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
            "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
        public ArgumentDescriptionAttribute ArgumentDescriptionAttribute
        {
            get
            {
                return this.argumentDescriptionAttributeField;
            }
            set
            {
                this.argumentDescriptionAttributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
            "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
        public ArgumentDirectionAttribute ArgumentDirectionAttribute
        {
            get
            {
                return this.argumentDirectionAttributeField;
            }
            set
            {
                this.argumentDirectionAttributeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
            "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
        public ArgumentEntityAttribute ArgumentEntityAttribute
        {
            get
            {
                return this.argumentEntityAttributeField;
            }
            set
            {
                this.argumentEntityAttributeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", IsNullable = false)]
    public partial class ArgumentRequiredAttribute
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", IsNullable = false)]
    public partial class ArgumentTargetAttribute
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", IsNullable = false)]
    public partial class ArgumentDescriptionAttribute
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", IsNullable = false)]
    public partial class ArgumentDirectionAttribute
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "clr-namespace:Microsoft.Xrm.Sdk.Workflow;assembly=Microsoft.Xrm.Sdk.Workflow, Ver" +
        "sion=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", IsNullable = false)]
    public partial class ArgumentEntityAttribute
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/netfx/2009/xaml/activities")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/netfx/2009/xaml/activities", IsNullable = false)]
    public partial class InArgument
    {

        private string typeArgumentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/winfx/2006/xaml")]
        public string TypeArguments
        {
            get
            {
                return this.typeArgumentsField;
            }
            set
            {
                this.typeArgumentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/winfx/2006/xaml")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/winfx/2006/xaml", IsNullable = false)]
    public partial class Members
    {

        private MembersProperty[] propertyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public MembersProperty[] Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
            }
        }
    }
}
