export default interface SubmissionInterface {
    canVote: boolean;
    hasVote: boolean;
    title: string;
    description: string;
    creatorName: string;
    participateNames: string[];
    votes: number;
    placement: number;
    isSubmitted: boolean;
    musicDetails: undefined | { link: string };
    movieDetails: undefined | { movieLink: string, trailerLink: string }
    shortStoryDetails: undefined | { story: string };
}

export interface SubmissionSmallInterface {
    id: number;
    title: string
    type: string
}

export interface SubmissionListGetInterface {
    userName?: string;
    competitionId?: number;
    phase?: string;
    sortByPlacement: boolean;
    sortByVoteCount: boolean;
    sortByDate: boolean;
}

export interface SubmissionListDetailsInterface {
    id: number;
    title: string;
    type: string;
    phase: string;
    placement: number;
    voteCount: number;
}