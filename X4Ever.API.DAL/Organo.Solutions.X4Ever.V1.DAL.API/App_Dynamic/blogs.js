var Blog = /** @class */ (function () {
    function Blog(methodName) {
        this.methodName = methodName;
        this.baseUrl = "https://mapp.oghq.ca/api/news/";
        this.url = this.baseUrl + methodName;
    }
    return Blog;
}());
function getNews() {
    var blog = new Blog("getbyactive");
}
//# sourceMappingURL=blogs.js.map