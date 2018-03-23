import {Component, OnInit} from '@angular/core';
import {FormControl} from '@angular/forms';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/throttleTime';
import 'rxjs/add/observable/fromEvent';
import {ApplicationApiClient} from '../../../api/application/application-api.client';
import {TranslateService} from '@ngx-translate/core';
import {Country} from '../../../services/common/country';
import {CommonService} from '../../../services/common/common.service';

@Component({
  selector: 'app-country-autocomplete',
  templateUrl: './country-autocomplete.component.html',
  styleUrls: ['./country-autocomplete.component.css']
})
export class CountryAutocompleteComponent implements OnInit {

  public countries: Country[] = [];
  public allCountriesList: Country[] = [];
  public isAutocompleteHidden: boolean;
  public isAreaListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public squareInput = false;
  public selectedCountry: string;
  public selectedCountryCode: string;

  constructor(private commonService: CommonService,
              private translateService: TranslateService) { }

  public ngOnInit() {
    this.isAreaListHovered = false;
    this.isSearchInputInFocus = false;
    this.hideCountryList();

    this.allCountriesList = this.commonService.countries;
    this.countries = this.allCountriesList;
    this.selectedCountry = '';
    this.selectedCountryCode = '';
  }

  public showCountryList() {
    this.isAutocompleteHidden = false;
    this.squareInput = true;
  }

  public hideCountryList() {
    if (this.isAreaListHovered) {
      return;
    }
    if (this.isSearchInputInFocus) {
      return;
    }
    this.isAutocompleteHidden = true;
    this.squareInput = false;
  }

  public countryListHoverStatusSwitch(status: boolean) {
    this.isAreaListHovered = status;
  }

  public searchInputFocusStatusSwitch(status: boolean) {
    this.isSearchInputInFocus = status;
  }

  public selectCountry(country) {
    this.selectedCountry = this.translateService.instant('Countries.' + country);
    this.selectedCountryCode = country;
    this.isAutocompleteHidden = true;
  }

  public onCountryInputChange(event) {
    this.selectedCountryCode = '';
    this.countries = this.allCountriesList.filter( c => c.name.toLowerCase().includes(event.target.value.toLowerCase()));
  }

}
