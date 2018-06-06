export class ColorHelper {

    public static coloredText(text: string, enteredText: string) {
        if (enteredText === '') {
            return text;
        }

        const lowerText: string = text.toLowerCase();
        const lowerSearch = enteredText.toLowerCase();

        let coloredString = '';
        const words = lowerText.split(lowerSearch);
        for (let i = 0; i < words.length; i++) {
            let startIndex = 0;
            if (words[i] !== '') {
                startIndex = lowerText.indexOf(words[i]) + words[i].length;
            } else {
                if (words.length > 1 && i > 0) {
                    startIndex = lowerText.indexOf(words[i - 1]) + words[i - 1].length;
                }
            }
            const lastIndex: number = startIndex + lowerSearch.length;
            const word = text.substring(startIndex, lastIndex);

            if (word === '') {
                continue;
            }

            const replacedWord = '<span style=\"background-color: #ffd038; color: black;\">' + word + '</span>';
            coloredString = text.replace(word, replacedWord);
        }

        if (words.every(i => i === '')) {
            const word = text.substring(0, lowerSearch.length);

            const replacedWord = '<span style=\"background-color: #ffd038; color: black;\">' + word + '</span>';
            coloredString = text.replace(word, replacedWord);
        }

        return coloredString === '' ? text : coloredString;
    }
}
