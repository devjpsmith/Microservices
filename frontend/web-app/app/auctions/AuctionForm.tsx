'use client'

import React, { useEffect } from 'react';
import { FieldValues, useForm } from "react-hook-form";
import { Button, TextInput } from "flowbite-react";
import { usePathname, useRouter } from "next/navigation";
import toast from "react-hot-toast";
import { createAuction, putAuction } from "@/app/actions/auctionActions";
import Input from "@/app/components/Input";
import DateInput from "@/app/components/DateInput";
import { Auction, RequestError } from "@/types";

type Props = {
    auction?: Auction;
    token: string;
};

function AuctionForm({token, auction}: Props) {
    const router = useRouter();
    const pathname = usePathname();
    const {
        register,
        handleSubmit,
        setFocus,
        control,
        reset,
        formState: {
            isSubmitting,
            isValid,
            isDirty,
            errors,
        }
    } = useForm({
        mode: 'onTouched'
    })

    useEffect(() => {
        console.log({auction});
        if (auction) {
            const {
                make, model, color, mileage, year
            } = auction;
            reset({make, model, color, mileage, year});
        }
        setFocus('make');
    }, [setFocus]);

    async function onSubmit(data: FieldValues) {
        try {
            const res = pathname === '/auctions/create'
                ? await createAuction(data, token)
                : await putAuction(auction?.id ?? '', data, token);
            if (res.error) {
                throw new RequestError(res.error.message, res.error.status);
            }
            router.push(`/auctions/details/${auction?.id || res.id}`);
        } catch (error) {
            if (error instanceof RequestError)
                toast.error(`${error.status} ${error.message}`);
        }
    }

    return (
        <form action="" className="flex flex-col mt-3" onSubmit={handleSubmit(onSubmit)}>
            <Input label={'Make'} name={'make'} control={control}
                   rules={{required: 'Make is required'}}/>
            <Input label={'Model'} name={'model'} control={control}
                   rules={{required: 'Model is required'}}/>
            <Input label={'Color'} name={'color'} control={control}
                   rules={{required: 'Color is required'}}/>

            <div className="grid grid-cols-2 gap-3">
                <Input label={'Year'} name={'year'} type={'number'} control={control}
                       rules={{required: 'Year is required'}}/>
                <Input label={'Mileage'} name={'mileage'} type={'number'} control={control}
                       rules={{required: 'Mileage is required'}}/>
            </div>

            {pathname === '/auctions/create' && (
                <>
                    <Input label={'Image URL'} name={'imageUrl'} control={control}
                           rules={{required: 'Image URL is required'}}/>

                    <div className="grid grid-cols-2 gap-3">
                        <Input label={'Reserve Price (enter 0 if no reserve)'} name={'reservePrice'} type={'number'} control={control}
                               rules={{required: 'Reserve Price is required'}}/>
                        <DateInput
                            label={'Auction End date/time'}
                            name={'auctionEnd'}
                            control={control}
                            dateFormat={'dd MMMM yyyy: h:mm a'}
                            showTimeSelect={true}
                            rules={{required: 'Auction End is required'}}/>
                    </div>
                </>
            )}

            <div className="flex justify-between">
                <Button outline color={'gray'}>Cancel</Button>
                <Button
                    isProcessing={isSubmitting}
                    disabled={!isValid}
                    type={'submit'}
                    outline
                    color={'success'}>Submit</Button>
            </div>
        </form>
    );
}

export default AuctionForm;