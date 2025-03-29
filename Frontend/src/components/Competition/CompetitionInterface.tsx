export default interface CompetitionInterface {
    id:number;
    title: string;
    description: string;
    isCreator: boolean;
    isModerator: boolean;
    canSubmit: boolean;
    creatorname: string;
    votingenabled: boolean;
    submissionsEnabled: boolean;
    isMusicCompetition: boolean;
    isMovieCompetition: boolean;

}

export interface CompetitionSmallInterface{
    id:number;
    title:string;
    type:string;
}