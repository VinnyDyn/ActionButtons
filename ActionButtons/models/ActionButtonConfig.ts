export class ActionButtonConfig
{
    public ActionUniqueName : string;
    public Tooltip : string;
    public WebResourceImage : string;
    public EnableHostEntityParameters : boolean;
    public DefaultParameters : DefaultParameter[]
}

export class DefaultParameter
{
    public Parameter : string;
    public DefaultValue : any
}