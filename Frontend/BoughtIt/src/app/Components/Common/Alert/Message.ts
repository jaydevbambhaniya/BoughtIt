export class Message{
    title:string='';
    message:string='';
    buttons:MessageButtonType[]=[];
}
export class MessageButtonType{
    text:string='';
    primary?:boolean
    action:() => void=()=>{}
}