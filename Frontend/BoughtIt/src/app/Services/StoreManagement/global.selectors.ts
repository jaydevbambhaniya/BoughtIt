import { createFeatureSelector, createSelector } from "@ngrx/store";
import { CartState, WishlistState } from "./global.state";
import { RouterReducerState } from "@ngrx/router-store";

const selectCartFeature = createFeatureSelector<CartState>('cart');
const selectRouterFeature = createFeatureSelector<RouterReducerState>('router');
const selectWishlistFeature = createFeatureSelector<WishlistState>('wishlist');

export const selectCartItems = createSelector(
  selectCartFeature,
  selectRouterFeature,
  (cart, router) => cart.items
);
export const selectCartCount = createSelector(
  selectCartFeature,
  selectRouterFeature,
  (cart, router) => cart.items?.length
);
export const selectWishlist = createSelector(
  selectWishlistFeature,
  selectRouterFeature,
  (wishlist, router) => wishlist.items
);
export const selectWishlistCount = createSelector(
  selectWishlistFeature,
  selectRouterFeature,
  (wishlist, router) => wishlist.items?.length
);
export const selectIsProductInCart = (productId: number) =>
  createSelector(selectCartItems, (cartProductIds) => {
    return cartProductIds?.some(p=>p.productId==productId)
});
export const selectIsProductInWishlist = (productId: number) =>
  createSelector(selectWishlist, (wishlist) => {
    return wishlist?.includes(productId)
});