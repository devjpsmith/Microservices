export type PagedResult<T> = {
    results: T[];
    pageCount: number;
    totalCount: number;
};

export type Auction = {
    reservePrice: number;
    seller: string;
    winner?: string;
    soldAmount?: number;
    currentHighBid?: number;
    createdAt: string;
    updatedAt: string;
    auctionEnd: string;
    status: string;
    make: string;
    model: string;
    year: number;
    color: string;
    mileage: number;
    imageUrl: string;
    id: string;
};

export class RequestError extends Error {
    constructor(message: string, public status: string) {
        super(message);
    }
}