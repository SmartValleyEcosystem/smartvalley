export interface ExpertRequest {
    transactionHash: string;
    address: string;
    email: string;
    name: string;
    about: string;
    isAvailable: boolean;
    areas: number[];
}
