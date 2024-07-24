'use server'

import {Auction, PagedResult} from "@/types";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { FieldValues } from "react-hook-form";
import { revalidatePath } from "next/cache";

export async function getData(queryUrl: string): Promise<PagedResult<Auction>> {
    return await fetchWrapper.get(`search/${queryUrl}`);
}

export async function UpdateAuctionTest(token: string) {
    const data = {
        mileage: Math.floor(Math.random() * 10000) + 1
    };
    return await fetchWrapper.put('auctions/bbab4d5a-8565-48b1-9450-5ac2a5c4a654', data, token);
}

export async function putAuction(id: string, data: FieldValues, token: string) {
    const res = await fetchWrapper.put(`auctions/${id}`, data, token);
    revalidatePath(`/auctions/${id}`);
    return res;
}

export async function createAuction(data: FieldValues, token: string) {
    return await fetchWrapper.post('auctions', data, token);
}

export async function getDetailedViewData(id: string): Promise<Auction> {
    return await fetchWrapper.get(`auctions/${id}`);
}

export async function deleteAuction(id: string, token: string) {
    return await fetchWrapper.del(`auctions/${id}`, token);
}