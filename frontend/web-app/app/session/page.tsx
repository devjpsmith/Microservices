import React from 'react';
import {getSession} from "@/app/actions/authActions";
import Heading from "@/app/components/Heading";
import AuthTest from "@/app/session/authTest";

async function Page() {
    const session = await getSession();
    return (
        <div>
            <Heading title={'Session dashboard'} />
            <div className="bg-blue-200 border-2 border-blue-500">
                <h3 className={'text-lg'} >Session data</h3>
                <pre className={'overflow-auto'}>{JSON.stringify(session, null, 2)}</pre>
            </div>
            <div className="mt-4">
                <AuthTest token={session?.user.token ?? ''} />
            </div>
        </div>
    );
}

export default Page;