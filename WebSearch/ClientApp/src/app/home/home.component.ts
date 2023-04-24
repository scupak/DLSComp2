import {Component} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public httpClient: HttpClient;
  public baseUrl: string = "http://localhost:9050";
  public CacheBaseUrl: string = "http://localhost:9051";
  public searchResult?: SearchResult = undefined;
  public searchTerms: string = "";
  public loading: boolean = false;
  public oldSearches: SearchResult[] = [];
  public selectedOldSearch?: SearchResult = undefined;

  constructor(http: HttpClient) {
    this.httpClient = http;
    this.getOldSearches();
  }

  public search(searchTerms: string) {
    this.loading = true;
    this.searchResult = undefined;
    console.log("Calling this url: " + this.baseUrl + '/LoadBalancer/Search?terms=');
    this.httpClient.get<SearchResult>(this.baseUrl + '/LoadBalancer/Search?terms=' + searchTerms + "&numberOfResults=" + 10).subscribe(result => {
      this.searchResult = result;
      this.getOldSearches();
      console.log(result);
    }, error => console.error(error));
  }

  public getOldSearchesTest() {
    var tempOldSearches: SearchResult[] = [{
      ellapsedMiliseconds: 3521.5122 ,
      SearchDateTime: new Date("2023-04-23T16:49:46.866988+00:00"),
      searchTerms: "Money",
      ignoredTerms: [],
      documents: [{id: BigInt(1) , path: "C:\\data\\maildir\\blair-l\\sent_items\\903.txt", numberOfAppearances: BigInt(1) }, {id: BigInt(2) , path: "C:\\data\\maildir\\hair-l\\sent_items\\903.txt", numberOfAppearances: BigInt(2) }]

    },
      {
        ellapsedMiliseconds: 421.5122 ,
        SearchDateTime: new Date("2023-04-24T16:49:46.866988+00:00"),
        searchTerms: "Honey",
        ignoredTerms: [],
        documents: [{id: BigInt(3) , path: "C:\\data\\amc\\blair-l\\sent_items\\903.txt", numberOfAppearances: BigInt(3) }, {id: BigInt(4) , path: "D:\\data\\maildir\\hair-l\\sent_items\\903.txt", numberOfAppearances: BigInt(4) }]

      }];

    this.oldSearches = tempOldSearches;
    this.loading = false;
  }

  public getOldSearches() {
    this.httpClient.get<SearchResult[]>(this.CacheBaseUrl + '/CacheLoadBalancer/GetAllFromCache').subscribe(result => {
      this.oldSearches = result;
      this.loading = false;
    }, error => console.error(error));
  }

  

  setOldSearches(oldSearchResult: SearchResult) {
    this.selectedOldSearch = oldSearchResult;
    this.searchResult = oldSearchResult;

  }
}

interface SearchResult {
  ellapsedMiliseconds: number;
  SearchDateTime: Date;
  searchTerms: string;
  ignoredTerms: string[];
  documents: Document[];

}

interface Document {

  id: bigint;
  path: string;
  numberOfAppearances: bigint;

}
