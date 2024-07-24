import React from 'react';
import Heading from "@/app/components/Heading";
import AuctionForm from "@/app/auctions/AuctionForm";
import { getSession } from "@/app/actions/authActions";

async function Create() {
    const session = await getSession();
    if (!session) throw new Error('Unable to get session');
    return (
        <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounder-lg">
            <Heading title={'Sell your car!'} subtitle={'Please enter the details of your car'} />
            <AuctionForm token={session.user.token}/>
        </div>
    );
}

export default Create;