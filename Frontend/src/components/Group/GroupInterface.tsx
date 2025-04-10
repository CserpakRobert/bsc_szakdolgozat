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
export interface GroupSmallListInterface {
    groups: [GroupSmallDetailsInterface];
    count: number;
}
export interface GroupSmallDetailsInterface {
    groupId: number;
    competitionId: number;
    ownerName: string;
    participateCount: number;
}