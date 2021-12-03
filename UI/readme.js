const { readFileSync, writeFileSync } = require("fs");

const s = readFileSync("settings.bsml", "utf8");

const tabs = /<tab.*?tab-name=['"](.*?)['"](.*?)<\/tab>/sg;
const options = /<(?<!text).*text="(.*?)".*?[\r\n]{0,2}.*?hover-hint="(.*?)"/g;

let out = "";

let m;
while(m = tabs.exec(s)) {
	const [, tab, content] = m;

	out += `### ${tab}\n\n`;

	while(m = options.exec(content)) {
		const [, tweak, description] = m;

		out += `- \`${tweak}\`: ${description}\n`
	}

	out += "\n";
}

const marker = "[tweaks]: <>";
let readme = readFileSync("../README.md", "utf8");

readme = readme.substr(0, readme.indexOf(marker) + marker.length);

readme += `\n${out}`;

writeFileSync("../README.md", readme);