import React from 'react';
import {AiOutlineCar} from "react-icons/ai";
import Search from "@/app/nav/Search";
import Logo from "@/app/nav/Logo";

function NavBar() {
    return (
        <header className='sticky top-0 z-50 flex justify-between bg-white p-5 items-center text-gray-800 shadow-md'>
            <div>
                <Logo />
            </div>
            <Search />
            <div>Login</div>
        </header>
    );
}

export default NavBar;