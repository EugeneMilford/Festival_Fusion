export interface Festival {
    festivalId: string,  // or number - API returns number
    festivalName: string,
    festivalDescription: string,
    theme: string,
    startDate: string,
    endDate: string,  // Changed from 'enddate' to 'endDate'
    sponsor: string
}
