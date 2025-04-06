export interface GroupDetailsInterface {
    competitionId: number;
    groupId: number;
    competitionTitle: string;
    ownerName: string;
    description: string;
    isClosed: boolean;
    participateNames: string[];
    inviteCount: number;
    requestCount: number;
}