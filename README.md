# ITSM Change Request Dataverse Plugin Demo  
Preventing Double Quotes From Being Saved

## Overview
This demo shows how to use a Dataverse plugin to enforce a simple data quality rule for an ITSM **Change Request** table:

> Do not allow double quotation marks (`"`) to be written to the database.

The plugin runs on create and update, inspects incoming values, and blocks the operation if any targeted field contains a double quote and replaces it with a single quote instead.
