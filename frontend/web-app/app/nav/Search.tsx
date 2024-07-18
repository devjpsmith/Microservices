'use client'

import React from 'react';
import {FaSearch} from "react-icons/fa";
import {useParamsStore} from "@/hooks/useParamsStore";
import {shallow} from "zustand/shallow";

function Search() {
    const setParams = useParamsStore(state => state.setParams);
    const params = useParamsStore(state => ({
        searchValue: state.searchValue
    }), shallow);
    const setSearchValue = useParamsStore(state => state.setSearchValue);

    function onChange(event: any) {
        setSearchValue(event.target.value);
    }

    function search() {
        setParams({searchTerm: params.searchValue});
    }

    return (
        <div className="flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm">
            <input
                type="text"
                onChange={onChange}
                onKeyDown={(e: any) => {
                    if (e.key === 'Enter') search();
                }}
                value={params.searchValue}
                className={`
                    flex-grow
                    pl-5
                    bg-transparent
                    focus:outline-none
                    border-transparent
                    focus:border-transparent
                    focus:ring-0
                    text-sm
                    text-gray-600`}
                placeholder={'Search for cars by make, model, or color'}
            />
            <button onClick={search}>
                <FaSearch size={34} className={'bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2'} />
            </button>
        </div>
    );
}

export default Search;