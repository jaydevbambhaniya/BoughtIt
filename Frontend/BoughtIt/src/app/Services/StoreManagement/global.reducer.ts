// cart.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { initialCartState, initialWishlistState } from './global.state';
import { fetchCart, fetchCartFailure, fetchCartSuccess, fetchWishlist, fetchWishlistFailure, fetchWishlistSuccess, updateCart, updateWishlist } from './global.actions';

export const cartReducer = createReducer(
  initialCartState,
  on(fetchCart, state => ({
    ...state,
    loading: false,
    error: null
  })),

  on(fetchCartSuccess, (state, { items }) => ({
    ...state,
    items,
    loading:false
  })),

  on(fetchCartFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  on(updateCart, (state, { item, isInCart }) => ({
    ...state,
    items: isInCart
      ? state.items.some((itm) => itm.productId === item.productId)
        ? state.items.map((itm) =>
            itm.productId === item.productId ? { ...itm, quantity: item.quantity } : itm
          )
        : [...state.items, item]
      : state.items.filter((itm) => itm.productId !== item.productId),
  }))
);

export const wishlistReducer = createReducer(
    initialWishlistState,
    on(fetchWishlist,state=>({
        ...state,
        loading:true
    })),
    on(fetchWishlistSuccess,(state, {items})=>({
        ...state,
        items,
        loading:false
    })),
    on(fetchWishlistFailure,(state,{error})=>({
        ...state,
        loading:false,
        error
    })),
    on(updateWishlist, (state, { userId,item, isInWishlist }) => ({
      ...state,
      items: isInWishlist
        ? state.items.includes(item)
          ? [...state.items]
          : [...state.items, item]
        : state.items.filter((itm) => itm !== item),
    }))
)
