'use client';

import React, { useState } from 'react';
import { Button } from "flowbite-react";
import { useRouter } from "next/navigation";
import { deleteAuction } from "@/app/actions/auctionActions";
import { RequestError } from "@/types";
import toast from "react-hot-toast";

type Props = {
    id: string;
    token: string;
};

function DeleteButton({id, token}: Props) {
    const [loading, setLoading] = useState(false)
    const router = useRouter();

    function doDelete() {
        setLoading(true);
        deleteAuction(id, token)
            .then(res => {
                if (res.error) throw new RequestError(res.statusText, res.status);
                router.push('/');
            })
            .catch(err => {
                if (err instanceof RequestError)
                    toast.error(`${err.status} ${err.message}`);
            })
            .finally(() => setLoading(false));
    }

    return (
        <Button
            color={'failure'}
            isProcessing={loading}
            onClick={doDelete}
        >
            Delete Auction
        </Button>
    );
}

export default DeleteButton;