use reqwest;
use scraper::{Html, Selector};
use tokio::fs;

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    // let text = fetch_events().await?;
    let text = load_events("tmp/events.html").await?;
    println!("{:?}", text);

    let document = Html::parse_document(&text);
    let entries = extract_entries(&document);

    for e in entries {
        println!("{:?}", e);
    }

    Ok(())
}

async fn fetch_events() -> Result<String, Box<dyn std::error::Error>> {
    let events_url = "https://adventofcode.com/events";
    let result = reqwest::get(events_url).await?;
    Ok(result.text().await?)
}

async fn load_events(file: &str) -> Result<String, Box<dyn std::error::Error>> {
    let contents = fs::read_to_string(file).await?;
    Ok(contents)
}

fn strip_year(text: &str) -> Option<i32> {
    text.strip_prefix("[")?.strip_suffix("]")?.parse::<i32>().ok()
}

fn strip_stars(text: &str) -> Option<i32> {
    text.strip_suffix("*")?.parse::<i32>().ok()
}

fn extract_entries(document: &Html) -> Vec<(i32, i32)> {
    let selector = Selector::parse("main > article > div").unwrap();

    document.select(&selector).filter_map(|el| {
        let t = el.text().collect::<Vec<_>>();
        let year = strip_year(t[0])?;
        let stars = strip_stars(t[2])?;
        let entry = (year, stars);
        Some(entry)
    }).collect::<Vec<_>>()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[tokio::test]
    async fn extract_entries_test() {
        let text = load_events("tmp/events.html").await.unwrap();
        let doc = Html::parse_document(&text);

        let entries = extract_entries(&doc);
        assert_eq!(entries,
            vec![
                (2022, 10),
                (2021, 44),
                (2020, 50),
                (2019, 42),
                (2018, 25),
                (2017, 40),
                (2016, 31),
                (2015, 40)
            ]);
    }
}