'use server'

import {Auction, PagedResult} from "@/types";

export async function getData(queryUrl: string): Promise<PagedResult<Auction>> {
    const res = await fetch(`http://localhost:6001/search${queryUrl}`);
    if (!res.ok) throw new Error('Failed to fetch data');

    return res.json();
}

export async function UpdateAuctionTest(token: string) {
    const data = {
        mileage: Math.floor(Math.random() * 10000) + 1
    };
    const res = await fetch('http://localhost:6001/auctions/bbab4d5a-8565-48b1-9450-5ac2a5c4a654', {
        method: 'PUT',
        headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json'},
        body: JSON.stringify(data)
    });
    if (!res.ok) return { status: res.status, message: res.statusText };

    return res.statusText;
}