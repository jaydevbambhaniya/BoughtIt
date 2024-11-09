import { inject, Injectable } from "@angular/core";
import { CartService } from "../../CartService/cart.service";
import { mergeMap, map, catchError, of, tap } from "rxjs";
import { fetchCart, fetchCartSuccess, fetchCartFailure, updateCart, fetchWishlistSuccess, fetchWishlist, fetchWishlistFailure, updateWishlist } from "../global.actions";
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { error } from "console";

@Injectable()
export class CartWishlistEffects {
  private actions$ = inject(Actions)
  constructor(
    private cartService: CartService
  ) {}

  fetchCart$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchCart),
      mergeMap(({ userId }) => {
        return this.cartService.getCartProducts(userId).pipe(
          map(items => {
            return fetchCartSuccess({ items });
          }),
          catchError(error => {
            return of(fetchCartFailure({ error: error.message || 'An unknown error occurred' }));
          })
        );
      })
    )
  );
  updateCart$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateCart),
        mergeMap(({ userId, item, isInCart }) => {
          return this.cartService.updateUserCart(userId, item, isInCart).pipe(
            tap((response) => {
              console.log('Cart updated successfully:', response);
            }),
            catchError((error) => {
              console.error('Error updating cart:', error);
              return of();
            })
          );
        })
      ),
    { dispatch: false }
  );
  
  fetchWishlist$ = createEffect(() =>
    this.actions$.pipe(
      ofType(fetchWishlist),
      mergeMap(({ userId }) => {
        return this.cartService.getUserWishlist(userId).pipe(
          map(items => {
            return fetchWishlistSuccess({ items });
          }),
          catchError(error => {
            return of(fetchWishlistFailure({ error: error.message || 'An unknown error occurred' }));
          })
        );
      })
    )
  );
  updateWishlist$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(updateWishlist),
        mergeMap(({ userId, item, isInWishlist }) => {
          return this.cartService.updateUserWishlist(userId, item, isInWishlist).pipe(
            tap((response) => {
              console.log('Cart updated successfully:', response);
            }),
            catchError((error) => {
              console.error('Error updating cart:', error);
              return of();
            })
          );
        })
      ),
    { dispatch: false }
  );
}