import { InputParameter } from "./InputParameter";
import { OutputParameter } from "./Outputparameter";

export class Action
{
    public Id : string;
    public InputParameters : InputParameter[];
    public OutputParameters : OutputParameter[];
    public UniqueName : string;
}