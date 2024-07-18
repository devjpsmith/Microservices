import Listings from "@/app/auctions/Listings";
import AuctionCard from "@/app/auctions/AuctionCard";
import {Auction, PagedResult} from "@/types";



export default async function Home() {
  return (
   <Listings />
  );
}
