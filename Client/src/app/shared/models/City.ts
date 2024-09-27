import { Country } from "./Country";

export interface City {
  id: number;
  name: string;
  countryId: number;
  country?: Country;
}
