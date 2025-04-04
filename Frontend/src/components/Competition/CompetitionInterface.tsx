export default interface CompetitionInterface {
    id: number;
    title: string;
    description: string;
    creatorname: string;
    phase: string;
    type: string;
    relevantDate: string;
}

export interface CompetitionSmallInterface{
    id:number;
    title:string;
    type:"movie"|"music"|"shortstory";
    phase:"preparation"|"voting"|"submit"|"finished"
    relevantDate:string
}