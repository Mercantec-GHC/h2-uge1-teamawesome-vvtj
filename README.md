# H2-U1
Pensum og opgave til uge 1 på H2 med MAGS  
-----
-----
**Vlad**: I developed a Blazor WebAssembly component that fetches gasoline and diesel price data via two API requests, deserializes it into a fuel model, and processes it in a Razor page. The data is mapped to a FuelViewModel, where I calculated statistical metrics such as the latest price, average, median, and variance for each fuel type. I then configured a BlazorBootstrap LineChart to visualize the data, combining and deduplicating date stamps for the x-axis and mapping price values for the y-axis, applying different colors for gasoline and diesel. I also created a table that displays both fuel types with their calculated statistics, and implemented error handling to display a warning message if data retrieval fails, ensuring that data is only loaded and rendered once for better performance.


**Victoria**:
Overview
I created a Blazor WebAssembly app, which retrieves country data from a backend API and displays it using Blazor Bootstrap components. Users can browse a list of countries, apply sorting/filtering, and view detailed information in a dashboard layout. Blazor’s data binding ensures that once API data is fetched, the UI updates automatically without manual refreshes.

Page Flow
1. User visits /countries
 - Countries.razor loads.
 - Calls the API to get a list of countries.
 - Displays them in a paginated, sortable, and filterable grid.

2. User clicks "View" on a country
 - Navigates to /countries/{countryId}.

3. CountryDashboard.razor loads
 - Calls the API for detailed data about the selected country.
 - Renders details in a responsive, styled card layout.

**Jasmin:**


**Tetiana:**
