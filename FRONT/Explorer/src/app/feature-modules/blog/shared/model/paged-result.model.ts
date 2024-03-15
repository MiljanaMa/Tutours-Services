export interface PagedResult<T> {
    results: T[],
    totalcount: number;
}