# COVIDShotCount
This application (the "Application") is an aggregator that tracks U.S. COVID-19 vaccination data. The data is aggregated from the website of Centers for Disease Control and Prevention (CDC).

# Schedule
The Application executes on a thirty-minute interval. If a change is detected in the data the CDC provides, it will perform the following functionalities:

- Post on Twitter the number of first doses and second doses administered
- Store the data (first doses administered, second doses administered, and doses distributed) into a database

In addition to the aggregation of the CDC's website, the Application has multiple routines that are executed on specific days. These routines are comprised of:

- Posting on Twitter a bar graph depicting the United States' progress in administering vaccines (Saturdays at 1 P.M. EST)
- Posting on Twitter a line graph depicting the United States' progress in establishing herd immunity (Sundays at 1 P.M. EST)

# Aspirations
Although I've accomplished the main component of this project, there are a few things yet in store:

- Develop a publically-accessible and intuitive website that will publicize exclusive vaccination data (that this Application aggregates) in a variety of different forms
- Expand automated publishing to Facebook and Instagram
- Further contribute to the data available to researchers

# Technologies
This project could not have been completed without the work of the many others who have contributed to the technologies the Application utilizes, including:

- [.NET Core](https://github.com/dotnet/core)
- [HTMLAgilityPack](https://github.com/zzzprojects/html-agility-pack)
- [MongoDB](https://github.com/mongodb/mongo-csharp-driver)
- [QuickChart](https://github.com/typpo/quickchart-csharp)
- [TweetinviAPI](https://github.com/linvi/tweetinvi)