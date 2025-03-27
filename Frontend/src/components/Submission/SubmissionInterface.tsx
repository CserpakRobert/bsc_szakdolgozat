export default interface SubmissionInterface {
    canVote: boolean;
    title: string;
    description: string;
    votes: number;
    musicDetails: undefined | { link: string };
    movieDetails: undefined | { movieLink: string, trailerLink: string }
}