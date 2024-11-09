import { CartProductDto } from "../../Models/CartProductDto";

export interface CartState {
    items: CartProductDto[]; 
    loading: boolean;
    error: string | null;
}
  
export const initialCartState: CartState = {
    items: [], 
    loading: false,
    error: null
};

export interface WishlistState{
    items: number[];
    loading: boolean;
    error: string|null
}

export const initialWishlistState: WishlistState = {
    items: [],
    loading: false,
    error: null,
}