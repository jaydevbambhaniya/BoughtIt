import { createAction, props } from '@ngrx/store';
import { CartProductDto } from '../../Models/CartProductDto';
import { create } from 'domain';

export const fetchCart = createAction(
    '[Cart] Fetch Cart',
    props<{ userId: number }>()  // Define userId in the payload
  );

export const fetchCartSuccess = createAction(
  '[Cart] Fetch Cart Success',
  props<{ items: CartProductDto[] }>()
);

export const fetchCartFailure = createAction(
  '[Cart] Fetch Cart Failure',
  props<{ error: any }>()
);

export const updateCart = createAction(
  '[Cart] Add To Cart',
  props<{ userId:number,item: CartProductDto,isInCart:boolean }>()
);

export const fetchWishlist = createAction('[Wishlist] Fetch Wishlist',
  props<{userId:number}>()
);

export const fetchWishlistSuccess = createAction(
  '[Wishlist] Fetch Wishlist Success',
  props<{ items: number[] }>()
);

export const fetchWishlistFailure = createAction(
  '[Wishlist] Fetch Wishlist Failure',
  props<{ error: any }>()
);

export const updateWishlist = createAction(
  '[Wishlist] Update Wishlist',
  props<{userId:number,item:number,isInWishlist:boolean}>()
)
