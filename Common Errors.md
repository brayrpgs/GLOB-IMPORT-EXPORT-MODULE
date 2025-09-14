# Common Errors

## ⚠️ CSV Requirements

The project is designed to accept **CSV files** from an application. Not all CSV files are compatible. To ensure a successful import, the CSV must meet the following criteria:

- ✅ **Language**: The file must be in **English**.
- ✅ **Source**: It must be **exported from the official Jira platform**.
- 📌 Refer to the **“Importation Process”** section for step-by-step instructions on exporting correctly.

## ❌ Unsupported Files

- Attempting to upload files in formats other than CSV (e.g., PDF, DOCX, PPT) will **not work**.
- The microservice **does not support importing multiple projects simultaneously**. Only one project can be imported per session.

## 🔧 Jira Customizations

Some data may not display as expected due to **customizations in Jira**, such as personalized issue types or states.

- You may notice default placeholders like **“Other”**.
- **Important**: This is **not an error**, but a control value used by the microservice to handle non-standard or unrecognized states.
- While it ensures the data is imported safely, it **may not appear visually ideal** in the application.

### 💡 Tips

- Always verify your CSV follows the official Jira export format.
- Avoid making multiple customizations in Jira that could affect standard issue fields.
- Use the microservice to import **one project at a time** to avoid conflicts.
