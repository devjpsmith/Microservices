'use client';

import React from 'react';
import { Button } from "flowbite-react";
import Link from "next/link";

type Props = {
    id: string;
};

function EditButton({id}: Props) {
    return (
        <Button outline>
            <Link href={`/auctions/update/${id}`}>Update Auction</Link>
        </Button>
    );
}

export default EditButton;