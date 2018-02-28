export interface EditExpertRequest {
    transactionHash?: string;
    address: string;
    email: string;
    name: string;
    about: string;
    isAvailable: boolean;
    areas: number[];
}