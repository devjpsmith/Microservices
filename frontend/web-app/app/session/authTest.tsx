'use client'

import React, {useState} from 'react';
import {UpdateAuctionTest} from "@/app/actions/auctionActions";
import {Button} from "flowbite-react";

type Props = {
    token: string;
};

function AuthTest({token}: Props) {
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState<any>();

    function doUpdate() {
        setResult(undefined);
        setLoading(true);
        UpdateAuctionTest(token)
            .then(res => setResult(res))
            .finally(() => setLoading(false));
    }
    return (
        <div className="flex items-center gap-4">
            <Button outline isProcessing={loading} onClick={doUpdate} >Test Auth</Button>
            <div>
                {JSON.stringify(result, null, 2)}
            </div>
        </div>
    );
}

export default AuthTest;