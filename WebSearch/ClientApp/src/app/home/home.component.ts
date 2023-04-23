import {Component} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public httpClient: HttpClient;
  public baseUrl: string = "http://localhost:9050";
  public searchResult?: SearchResult = undefined;
  public searchTerms: string = "";
  public loading: boolean = false;


  constructor(http: HttpClient) {
    this.httpClient = http;
  }

  public search(searchTerms: string) {
    this.loading = true;
    this.searchResult = undefined;
    console.log("Calling this url: " + this.baseUrl + '/LoadBalancer/Search?terms=');
    this.httpClient.get<SearchResult>(this.baseUrl + '/LoadBalancer/Search?terms=' + searchTerms + "&numberOfResults=" + 10).subscribe(result => {
      this.searchResult = result;
      this.loading = false;
      console.log(result);
    }, error => console.error(error));
  }

}

interface SearchResult {
  ellapsedMiliseconds: number;

  SearchDateTime: Date;
  ignoredTerms: string[];
  documents: Document[];

}

interface Document {

  id: bigint;
  path: string;
  numberOfAppearances: bigint;

}
