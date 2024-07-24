'use client'

import React from 'react';
import { Toaster } from "react-hot-toast";

function ToasterProvider() {
    return (
        <Toaster position={'bottom-right'}></Toaster>
    );
}

export default ToasterProvider;
