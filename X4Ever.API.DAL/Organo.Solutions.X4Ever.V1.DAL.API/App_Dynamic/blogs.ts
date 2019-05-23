class Blog {
    url: string;
    private baseUrl: string = "https://mapp.oghq.ca/api/news/";
    constructor(public methodName: string) {
        this.url = this.baseUrl + methodName;
    }
}

interface INews {
    Header: string;
    Body: string;
    PostDate: Date;
    PostedBy: string;
    NewsImage: string;
    NewsImagePosition: string;
    Active: boolean;
    ModifyDate: Date;
    ModifiedBy: string;
    LanguageCode: string;
    ApplicationId: number;
}

function getNews() {
    var blog = new Blog("getbyactive");
}