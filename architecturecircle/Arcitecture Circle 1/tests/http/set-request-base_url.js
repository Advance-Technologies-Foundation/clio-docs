const baseUrl = request.environment.get("base_url");
const isNetCore = request.environment.get("isNetCore");
console.log("isNetCore:" + isNetCore);
console.log("baseUrl:" + baseUrl);

if (isNetCore === "true") {
	console.log("Set base_url to:" + baseUrl);
	client.global.set("base_url", baseUrl);
} else {
	console.log("Set base_url to:" + `${baseUrl}/0`);
	client.global.set("base_url", `${baseUrl}/0`);
}