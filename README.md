# Jobsity .Net Challenge

Created a solution with .NET Core 3.1 Projects
- FinanceChat

Chat Project using signalR and SQLServer. Also use .Net Identity for user authentication.

- FinanceCommon

Library with common Models throught projects

- StockBot

WebApi Project to handle stock option requests

- TestProject1

XUnit testing

## Configuration
- FinanceChat
  - Edit **appsettings.json**:
    - "*StockBotURL*": Set the URL where the StockBot (project above) listen.
    - "*ConnectionStrings > DefaultConnection*": Set your SQLServer Connection String

## Mandatory Features
- [x] Allow registered users to log in and talk with other users in a chatroom.
- [x] Allow users to post messages as commands into the chatroom with the following format **/stock=stock_code**
- [x] Create a *decoupled bot* that will call an API using the **stock_code** as a parameter (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here *aapl.us* is the stock_code)
- [x] The bot should parse the received CSV file and then it should send a message back into the chatroom using a message broker like RabbitMQ. The message will be a stock quote using the following format: "APPL.US quote is $93.42 per share". The post owner will be the bot.
- [x] Have the chat messages ordered by their timestamps and show only the last 50 messages.
- [x] Unit test the functionality you prefer.

## Bonus (Optional)
- [x] Use .NET identity for users authentication
- Handle messages that are not understood or any exceptions raised within the bot.
- Build an installer.
