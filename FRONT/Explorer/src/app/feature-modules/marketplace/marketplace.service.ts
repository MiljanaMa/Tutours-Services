import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TourPreference } from './model/tour-preference.model';
import { environment } from 'src/env/environment';
import { Observable } from 'rxjs';
import { PagedResult } from '../tour-execution/shared/model/paged-result.model';
import { Tour } from '../tour-authoring/model/tour.model';
import { OrderItem } from './model/order-item.model';
import { PagedResults } from '../../shared/model/paged-results.model';
import { ShoppingCart } from './model/shopping-cart.model';
import { Keypoint } from '../tour-authoring/model/keypoint.model';
import { Object } from '../tour-authoring/model/object.model';
import { TourReview } from '../tour-execution/model/tour-review.model';
import { TourPurchaseToken } from './model/tour-purchase-token.model';

import { Bundle } from './model/bundle.model';
import { BundlePrice } from './model/bundle-price.model';

import { Wallet } from './model/wallet.model';
import { WishListItem } from './model/wish-list-item.model';
import { WishList } from './model/wish-list.model';
import { Discount } from './model/discount.model';
import { Tourist } from './model/tourist-model';
import { Coupon } from './model/coupon-model';
import { TourDiscount } from './model/tour-discount.model';


@Injectable({
  providedIn: 'root',
})
export class MarketplaceService {
  private readonly apiUrl = `${environment.apiHost}tourist`;
  private readonly tourApiUrl = `${environment.apiHost}marketplace/tours`;
  private readonly filterApiUrl = `${environment.apiHost}marketplace/tours/filter`;
  private readonly tourReviewApiUrl = `${environment.apiHost}tourexecution/tourreview`;
  private readonly userApiUrl = `${environment.apiHost}administration/users`;
  private readonly couponApiUrl = `${environment.apiHost}marketplace/coupons`;

  constructor(private http: HttpClient) {}

  getTourPreference(): Observable<TourPreference> {
    return this.http.get<TourPreference>(`${this.apiUrl}/tourPreference`);
  }

  deleteTourPreference(): Observable<TourPreference> {
    return this.http.delete<TourPreference>(`${this.apiUrl}/tourPreference`);
  }

  addTourPreference(
    tourPreference: TourPreference,
  ): Observable<TourPreference> {
    return this.http.post<TourPreference>(
      `${this.apiUrl}/tourPreference`,
      tourPreference,
    );
  }

  updateTourPreference(
    tourPreference: TourPreference,
  ): Observable<TourPreference> {
    return this.http.put<TourPreference>(
      `${this.apiUrl}/tourPreference`,
      tourPreference,
    );
  }

  /* Tour */
  getPublishedTours(): Observable<PagedResult<Tour>> {
    return this.http.get<PagedResult<Tour>>(`${this.tourApiUrl}`);
  }

  getFilteredTours(
    page: number,
    pageSize: number,
    currentLatitude: number,
    currentLongitude: number,
    filterRadius: number,
  ): Observable<PagedResult<Tour>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('CurrentLatitude', currentLatitude.toString())
      .set('CurrentLongitude', currentLongitude.toString())
      .set('FilterRadius', filterRadius.toString());
    return this.http.get<PagedResult<Tour>>(`${this.filterApiUrl}`, { params });
  }

  getPublicObjects(
    page: number,
    pageSize: number,
    currentLatitude: number,
    currentLongitude: number,
    filterRadius: number,
  ): Observable<PagedResult<Object>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('CurrentLatitude', currentLatitude.toString())
      .set('CurrentLongitude', currentLongitude.toString())
      .set('FilterRadius', filterRadius.toString());
    return this.http.get<PagedResult<Object>>(
      environment.apiHost + 'tourist/object/filtered',
      { params },
    );
  }

  getPublicKeyPoints(
    page: number,
    pageSize: number,
    currentLatitude: number,
    currentLongitude: number,
    filterRadius: number,
  ): Observable<PagedResult<Keypoint>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('CurrentLatitude', currentLatitude.toString())
      .set('CurrentLongitude', currentLongitude.toString())
      .set('FilterRadius', filterRadius.toString());
    return this.http.get<PagedResult<Keypoint>>(
      environment.apiHost + 'tourist/publicKeypoint/filtered',
      { params },
    );
  }

  addOrderItem(orderItem: OrderItem): Observable<OrderItem> {
    return this.http.post<OrderItem>(
      environment.apiHost + 'tourist/orderItems',
      orderItem,
    );
  }

  createWallet(wallet: Wallet): Observable<Wallet> {
    return this.http.post<Wallet>(
      environment.apiHost + 'tourist/wallet',
      wallet,
    );
  }

  getAllOrders(): Observable<PagedResult<OrderItem>> {
    return this.http.get<PagedResult<OrderItem>>(`${this.apiUrl}/orderItems`);
  }

  getAllWallets(): Observable<PagedResult<OrderItem>> {
    return this.http.get<PagedResult<OrderItem>>(`${this.apiUrl}/wallet`);
  }

  getOrdersForUser(): Observable<PagedResults<OrderItem>> {
    return this.http.get<PagedResults<OrderItem>>(
      environment.apiHost + 'tourist/orderItems/byUser',
    );
  }

  updateShoppingCart(shoppingCart: ShoppingCart): Observable<ShoppingCart> {
    return this.http.put<ShoppingCart>(
      `${this.apiUrl}/shoppingCart`,
      shoppingCart,
    );
  }

  getShoppingCartForUser(): Observable<ShoppingCart> {
    return this.http.get<ShoppingCart>(
      environment.apiHost + 'tourist/shoppingCart/byUser',
    );
  }

  getWalletForUser(): Observable<Wallet> {
    return this.http.get<Wallet>(environment.apiHost + 'tourist/wallet/byUser');
  }

  getWalletForUserId(userId: number): Observable<Wallet> {
    return this.http.get<Wallet>(
      environment.apiHost + 'tourist/wallet/byUser/' + userId,
    );
  }

  addCoins(wallet: Wallet): Observable<Wallet> {
    const url = `${this.apiUrl}/wallet/addCoins/${wallet.id}`;
    return this.http.put<Wallet>(url, wallet);
  }

  deleteOrderItem(orderItemId: number): Observable<OrderItem> {
    return this.http.delete<OrderItem>(
      environment.apiHost + 'tourist/orderItems/' + orderItemId,
    );
  }

  getArchivedAndPublishedTours(): Observable<PagedResults<Tour>> {
    return this.http.get<PagedResults<Tour>>(
      `${this.tourApiUrl}/arhived-published`,
    );
  }

  getReviewsByTour(tourId: number): Observable<PagedResults<TourReview>> {
    return this.http.get<PagedResults<TourReview>>(
      `${this.tourReviewApiUrl}/tour/${tourId}`,
    );
  }

  buyShoppingCart(id: number, selectedCoupons: Coupon[]): Observable<void> {
    return this.http.post<void>(
      `${this.tourApiUrl}/token/${id}`,
      selectedCoupons,
    );
  }

  calculateAverageRate(tourReviews: TourReview[]): Observable<number> {
    return this.http.post<number>(
      this.tourReviewApiUrl + '/averageRate',
      tourReviews,
    );
  }

  getPurchasedTours(): Observable<PagedResults<TourPurchaseToken>> {
    return this.http.get<PagedResults<TourPurchaseToken>>(
      `${this.tourApiUrl}/token`,
    );
  }

  checkIfPurchased(tourId: number): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.tourApiUrl}/token/check-purchase/${tourId}`,
    );
  }


  createBundle(bundle:Bundle) : Observable<Bundle>{
    return this.http.post<Bundle>(`${environment.apiHost}bundles`, bundle);
  }
  getToursByAuthor(): Observable<PagedResult<Tour>>{
    return this.http.get<PagedResult<Tour>>(`${environment.apiHost}author/tours/author?page=0&pageSize=0`);
  }

  getBundle(id:number): Observable<Bundle>{
    return this.http.get<Bundle>(`${environment.apiHost}bundles/${id}`);
  }

  getAllBundles(): Observable<PagedResult<Bundle>>{
    return this.http.get<PagedResult<Bundle>>(`${environment.apiHost}bundles?page=0&pageSize=0`);
  }

  updateBundle(): Observable<Bundle>{
    return this.http.put<Bundle>(`${environment.apiHost}bundles`,null);
  }

  createPriceForBundle(price:BundlePrice) : Observable<BundlePrice>{
    return this.http.post<BundlePrice>(`${environment.apiHost}bundlePrice`, price);

  }
  getPriceForBundle(bundleId:number) : Observable<BundlePrice>{
    return this.http.get<BundlePrice>(`${environment.apiHost}bundlePrice/${bundleId}`);
  }
  addTourToBundle(tourId:number, bundleId:number) : Observable<Bundle>{
    return this.http.post<Bundle>(`${environment.apiHost}bundles/AddTourToBundle/${tourId}/${bundleId}`, null);
  }
  deleteTourFromBundle(tourId:number, bundleId:number): Observable<Bundle>{
    return this.http.delete<Bundle>(`${environment.apiHost}bundles/RemoveTourFromBundle?tourId=${tourId}&bundleId=${bundleId}`);
  }

  deleteBundle(id:number) :Observable<Bundle>{
    return this.http.delete<Bundle>(`${environment.apiHost}bundles/${id}`);
  }

  publishBundle(id:number) : Observable<Bundle>{
    return this.http.put<Bundle>(`${environment.apiHost}bundles/publish/${id}`, null);
  }
  archiveBundle(id:number) : Observable<Bundle>{
    return this.http.put<Bundle>(`${environment.apiHost}bundles/archive/${id}`, null);
  }

  getCustomTours(): Observable<PagedResult<Tour>> {
    return this.http.get<PagedResult<Tour>>(`${this.tourApiUrl}/custom`);
  }

  updateWallet(wallet: Wallet): Observable<Wallet> {
    return this.http.put<Wallet>(
      environment.apiHost + 'tourist/wallet/' + wallet.id,
      wallet,
    );
  }
  
  calculateBundlePrice(bundleId:number) : Observable<number> {
    return this.http.get<number>(`${environment.apiHost}bundles/calculate?bundleId=${bundleId}`);
  }

  getWishListItemsForUser(): Observable<PagedResults<WishListItem>> {
    return this.http.get<PagedResults<WishListItem>>(environment.apiHost + 'tourist/itemswishlist');
  }

  getWishListForUser(): Observable<WishList> {
    return this.http.get<WishList>(environment.apiHost + 'tourist/wishlist/byUser');
  }

  getTourById(tourId: number): Observable<Tour>{
    return this.http.get<Tour>(`${environment.apiHost}tourist/tours/${tourId}`);
  }

  getAllWishListItems(): Observable<PagedResult<WishListItem>> {
    return this.http.get<PagedResult<WishListItem>>(environment.apiHost + 'tourist/itemswishlist');
  }

  addWishListItem(wishListItem: WishListItem): Observable<WishListItem> {
    return this.http.post<WishListItem>(environment.apiHost + 'tourist/itemswishlist', wishListItem);
  }

  deleteWishListItem(wishListItemId: number): Observable<WishListItem> {
    return this.http.delete<WishListItem>(environment.apiHost + 'tourist/itemswishlist/' + wishListItemId);
  }

  getToursOnDiscount(): Observable<number[]> {
    return this.http.get<number[]>(
      environment.apiHost + 'marketplace/discounts/sorted',
    );
  }

  addDiscount(discount: Discount): Observable<Discount> {
    return this.http.post<Discount>(
      `${environment.apiHost}marketplace/discounts`,
      discount,
    );
  }

  updateDiscount(discount: Discount): Observable<Discount> {
    return this.http.put<Discount>(
      environment.apiHost + 'marketplace/discounts',
      discount,
    );
  }

  removeDiscount(id: number): Observable<Discount> {
    return this.http.delete<Discount>(
      `${environment.apiHost}marketplace/discounts/${id}`,
    );
  }

  getDiscountsByAuthor(): Observable<PagedResult<Discount>> {
    return this.http.get<PagedResult<Discount>>(
      environment.apiHost + 'marketplace/discounts/author-discounts',
    );
  }

  getDiscountedTours(discountId: number): Observable<number[]> {
    return this.http.get<number[]>(
      `${environment.apiHost}marketplace/tour-discount/tours/${discountId}`,
    );
  }

  getPublishedByAuthor(): Observable<PagedResult<Tour>> {
    return this.http.get<PagedResult<Tour>>(
      `${this.tourApiUrl}/author-published`,
    );
  }

  getAllDiscounts(): Observable<PagedResult<Discount>> {
    return this.http.get<PagedResult<Discount>>(
      environment.apiHost + 'marketplace/discounts',
    );
  }

  getAllTourists(): Observable<PagedResults<Tourist>> {
    return this.http.get<PagedResults<Tourist>>(
      `${this.userApiUrl}/allTourists`,
    );
  }

  getAllToursForAuthor(authorId: number): Observable<PagedResults<Tour>> {
    return this.http.get<PagedResults<Tour>>(
      `${this.tourApiUrl}/allToursForAuthor/${authorId}`,
    );
  }

  createCoupon(coupon: Coupon): Observable<PagedResult<Coupon>> {
    return this.http.post<PagedResult<Coupon>>(`${this.couponApiUrl}`, coupon);
  }

  getCouponsForTourAndTourist(
    tourId: number,
    touristId: number,
  ): Observable<PagedResults<Coupon>> {
    return this.http.get<PagedResults<Coupon>>(
      `${this.couponApiUrl}/getForTourAndTourist/${tourId}/${touristId}`,
    );
  }

  getCouponsForAuthor(authorId: number): Observable<PagedResults<Coupon>> {
    return this.http.get<PagedResults<Coupon>>(
      `${this.couponApiUrl}/getForAuthor/${authorId}`,
    );
  }

  deleteCoupon(id: number) {
    return this.http.delete<void>(`${this.couponApiUrl}/${id}`);
  }

  editCoupon(coupon: Coupon): Observable<PagedResult<Coupon>> {
    return this.http.put<PagedResult<Coupon>>(`${this.couponApiUrl}`, coupon);
  }

  addTourToDiscount(tourDiscount: TourDiscount): Observable<TourDiscount> {
    return this.http.post<TourDiscount>(
      environment.apiHost + 'marketplace/tour-discount',
      tourDiscount,
    );
  }

  removeTourFromDiscount(tourId: number): Observable<any> {
    return this.http.delete(
      environment.apiHost + `marketplace/tour-discount/${tourId}`,
    );
  }
}
