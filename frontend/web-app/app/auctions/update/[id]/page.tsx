import React from 'react';
import Heading from "@/app/components/Heading";
import AuctionForm from "@/app/auctions/AuctionForm";
import { fetchWrapper } from "@/lib/fetchWrapper";
import { id } from "date-fns/locale";
import { getSession } from "@/app/actions/authActions";
import { getDetailedViewData } from "@/app/actions/auctionActions";

async function Update({params}: { params: { id: string}}) {
    const session = await getSession();
    if (!session) throw new Error('Unable to get session');
    const data = await getDetailedViewData(params.id);
    return (
        <div className="mx-auto max-w-[65%] shadow-lg p-10 bg-white rounded-lg">
            <Heading title={'Update your auction'} subtitle={'Plead update the details of your car'} />
            <AuctionForm auction={data} token={session.user.token} />
        </div>
    );
}

export default Update;