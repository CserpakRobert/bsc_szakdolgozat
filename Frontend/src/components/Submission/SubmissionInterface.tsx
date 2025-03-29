export default interface SubmissionInterface {
    canVote: boolean;
    hasVote: boolean;
    title: string;
    description: string;
    votes: number;
    musicDetails: undefined | { link: string };
    movieDetails: undefined | { movieLink: string, trailerLink: string }
}

export interface SubmissionSmallInterface{
    id:number;
    title:string
    type:string
}