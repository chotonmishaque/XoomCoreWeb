
// Create a container for the toasts
const toastContainer = document.createElement('div');
toastContainer.classList.add('toast-container');
toastContainer.style.zIndex = 9999;

document.body.appendChild(toastContainer);

// Function to create and show a toast
function toastMessage({ className, responseCode, title, message }) {

    const toastPlacementExample = document.createElement('div');
    toastPlacementExample.classList.add('bs-toast', 'toast', 'toast-placement-ex', 'end-0', 'm-2', className, 'show');
    toastPlacementExample.setAttribute('role', 'alert');
    toastPlacementExample.setAttribute('aria-live', 'assertive');
    toastPlacementExample.setAttribute('aria-atomic', 'true');
    toastPlacementExample.setAttribute('data-delay', '2000');

    const toastHeader = document.createElement('div');
    toastHeader.classList.add('toast-header');

    const toastIcon = document.createElement('i');
    toastIcon.classList.add('bx', 'bx-bell', 'me-2');
    toastHeader.appendChild(toastIcon);

    const toastTitle = document.createElement('div');
    toastTitle.classList.add('me-auto', 'fw-semibold');
    toastTitle.textContent = title;
    toastHeader.appendChild(toastTitle);

    const toastTimestamp = document.createElement('small');
    toastTimestamp.textContent = 'Response Code : ' + responseCode;
    toastHeader.appendChild(toastTimestamp);

    const toastCloseBtn = document.createElement('button');
    toastCloseBtn.classList.add('btn-close');
    toastCloseBtn.setAttribute('type', 'button');
    toastCloseBtn.setAttribute('data-bs-dismiss', 'toast');
    toastCloseBtn.setAttribute('aria-label', 'Close');
    toastHeader.appendChild(toastCloseBtn);

    const toastBody = document.createElement('div');
    toastBody.classList.add('toast-body');
    toastBody.textContent = message;

    toastPlacementExample.appendChild(toastHeader);
    toastPlacementExample.appendChild(toastBody);

    toastContainer.appendChild(toastPlacementExample);

    const toastPlacement = new bootstrap.Toast(toastPlacementExample);
    toastPlacement.show();

    const toasts = toastContainer.querySelectorAll('.bs-toast');
    let offset = 0;
    for (let i = 0; i < toasts.length; i++) {
        const toast = toasts[i];
        toast.style.bottom = `${offset}px`;
        offset += toast.offsetHeight + 10;
    }

    setTimeout(() => {
        toastPlacement.dispose();
        toastContainer.removeChild(toastPlacementExample);
    }, 10000);
};

function showToast(responseCode, messageType, message) {
    switch (messageType) {
        case "Success":
            toastMessage({ className: 'bg-primary', responseCode: responseCode, title: messageType, message: message })
            break;
        case "Warning":
            toastMessage({ className: 'bg-warning', responseCode: responseCode, title: messageType, message: message })
            break;
        case "Error":
            toastMessage({ className: 'bg-danger', responseCode: responseCode, title: messageType, message: message })
            break;
        default:
            toastMessage({ className: 'bg-secondary', responseCode: responseCode, title: messageType, message: message })
            break;
    }
};


let spinner = document.getElementById("spinner");
spinner.style.display = 'none';
async function sendRequest(url, method, data) {
    try {
        const options = {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            }
        };
        if (data) {
            options.body = JSON.stringify(data);
        }
        const response = await fetch(url, options);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const result = await response.json();
        return result;
    } catch (error) {
        console.error(error);
    }
    finally {
        spinner.style.display = 'none';
    }
};

function groupBy(data, keys) {
    return Object.values(data.reduce((groups, item) => {
        const groupKey = keys.map(key => key.split('.').reduce((obj, k) => obj && obj[k], item)).join('_');
        if (!groups[groupKey]) {
            groups[groupKey] = [];
        }
        groups[groupKey].push(item);
        return groups;
    }, {}));
};

function createMenuCategory(icon, menuText, linkHref) {
    const menuItem = document.createElement("li");
    menuItem.classList.add("menu-item");

    const menuLink = document.createElement("a");
    menuLink.href = linkHref || "javascript:void(0);";
    menuLink.classList.add("menu-link", "menu-toggle");

    const menuIcon = document.createElement("i");
    menuIcon.className = "menu-icon tf-icons " + icon;
    menuLink.appendChild(menuIcon);

    const menuDiv = document.createElement("div");
    menuDiv.textContent = menuText;
    menuLink.appendChild(menuDiv);

    menuItem.appendChild(menuLink);

    return menuItem;
}

function createMenuItem(menuText, linkHref) {
    const subMenuItem = document.createElement("li");
    subMenuItem.classList.add("menu-item");

    const subMenuLink = document.createElement("a");
    subMenuLink.href = linkHref || "javascript:void(0);";
    subMenuLink.classList.add("menu-link");

    const subMenuDiv = document.createElement("div");
    subMenuDiv.textContent = menuText;
    subMenuLink.appendChild(subMenuDiv);

    subMenuItem.appendChild(subMenuLink);

    return subMenuItem;
};

document.addEventListener("DOMContentLoaded", async function () {
    const getUserUrl = "/GetUser";
    const data = await sendRequest(getUserUrl, "GET");
    if (data) {

        const keysToGroupBy = ['menu.name'];
        const groupedMenuItems = groupBy(data.menuList, keysToGroupBy);

        var menuContainer = document.getElementById("menu");
        menuContainer.length = 0;
        groupedMenuItems.forEach(function (rootMenu) {
            const mainMenu = createMenuCategory(rootMenu[0].menu.icon, rootMenu[0].menu.name, "javascript:void(0);");

            if (rootMenu.length > 0) {
                const subMenuContainer = document.createElement("ul");
                subMenuContainer.classList.add("menu-sub");

                rootMenu.forEach(function (menuItem) {
                    const subMenuItem = createMenuItem(menuItem.name, menuItem.url);
                    subMenuContainer.appendChild(subMenuItem);
                    if (menuItem.activeMenuItem) {
                        subMenuItem.classList.add("active");
                        mainMenu.classList.add("active", "open");
                    }
                });
                mainMenu.appendChild(subMenuContainer);
            }
            menuContainer.appendChild(mainMenu);
        });

    }
});
async function generateTable({ data, tableConfig }) {

    let itemsPerPage = tableConfig.itemsPerPage;
    let searchText = tableConfig.searchText;
    let currentPage = tableConfig.currentPage;
    let totalPages = Math.ceil(tableConfig.totalItems / itemsPerPage);
    if (!Boolean(data)) {
        data = [];
    }

    const rowDiv = document.createElement('div');
    rowDiv.className = 'row mt-2';

    const col1Div = document.createElement('div');
    col1Div.className = 'col-2 text-start';

    const selectElement = document.createElement('select');
    selectElement.className = 'form-select';
    for (const value of tableConfig.limitValues) {
        const option = document.createElement('option');
        option.value = value;
        option.textContent = value;
        selectElement.appendChild(option);
    }
    const option = document.createElement('option');
    option.value = tableConfig.totalItems;
    option.textContent = "ALL";
    selectElement.appendChild(option);
    selectElement.addEventListener('change', async (event) => {
        itemsPerPage = +event.target.value;
        if (itemsPerPage == tableConfig.totalItems) {
            currentPage = 1;
        }
        updateTable();
    });
    selectElement.value = itemsPerPage;
    col1Div.appendChild(selectElement);

    const col2Div = document.createElement('div');
    col2Div.className = 'offset-7 col-3 text-end';

    const searchInputDiv = document.createElement('div');
    searchInputDiv.className = "input-group";
    const searchInput = document.createElement('input');
    searchInput.type = 'text';
    searchInput.className = 'form-control';
    searchInput.placeholder = 'Search..';
    searchInput.id = "searchInput";
    searchInput.value = searchText;
    searchInputDiv.appendChild(searchInput);
    const searchInputButton = document.createElement('button');
    searchInputButton.type = 'button';
    searchInputButton.className = "btn btn-outline-primary";
    searchInputButton.textContent = "Filter";
    searchInputDiv.appendChild(searchInputButton);


    searchInput.addEventListener('keyup', async (event) => {
        if (event.key === 'Enter' || event.keyCode === 13) {
            currentPage = 1;
            searchText = event.target.value;
            updateTable();
        }
    });
    searchInputButton.addEventListener('click', async (event) => {
        currentPage = 1;
        searchText = searchInput.value;
        updateTable();
    });
    col2Div.appendChild(searchInputDiv);

    rowDiv.appendChild(col1Div);
    rowDiv.appendChild(col2Div);

    const parentElement = document.body;
    parentElement.appendChild(rowDiv);

    const tableDiv = document.createElement('div');
    tableDiv.className = "table-responsive";

    const table = document.createElement('table');
    table.className = "col-12 table table-hover exportToExcel";
    tableDiv.appendChild(table);

    // Add thead
    const thead = table.createTHead();
    const headerRow = thead.insertRow();

    // Add headers

    if (tableConfig.actionButtons.length > 0) {
        const actionHeader = document.createElement('th');
        actionHeader.textContent = 'Action';
        headerRow.appendChild(actionHeader);
    }

    const slth = document.createElement('th');
    slth.textContent = "SL";
    headerRow.appendChild(slth);

    tableConfig.columns.forEach(column => {
        const th = document.createElement('th');
        th.textContent = column.label;
        headerRow.appendChild(th);
    });

    thead.appendChild(headerRow);
    table.appendChild(thead);

    const tbody = table.createTBody();

    let serialNo = 0;
    data.forEach((rowData, index) => {
        const tr = document.createElement('tr');

        //actions
        if (tableConfig.actionButtons.length > 0) {
            const actionCell = document.createElement('td');
            tableConfig.actionButtons.forEach(button => {
                const buttonElement = createActionButton(button.label, button.icon, button.style, () => button.action(rowData));
                if (!button.enabled) {
                    buttonElement.disabled = true;
                }
                actionCell.appendChild(buttonElement);
            });
            tr.appendChild(actionCell);
        };

        //serial
        const td = document.createElement('td');
        serialNo = (itemsPerPage * currentPage) - itemsPerPage + index + 1;
        td.appendChild(document.createTextNode(serialNo));
        tr.appendChild(td);


        // Add cells
        tableConfig.columns.forEach(column => {
            const td = document.createElement('td');

            if (column.renderElement && typeof column.renderElement === 'function') {
                const element = column.renderElement(rowData);
                td.appendChild(element);
            } else if (column.customRender && typeof column.customRender === 'function') {
                const value = column.customRender(rowData);
                td.appendChild(document.createTextNode(value));
            } else {
                const value = rowData[column.key];
                td.appendChild(document.createTextNode(value));
            }

            tr.appendChild(td);
        });


        tbody.appendChild(tr);
    });


    const caption = document.createElement('caption');
    caption.className = "ms-4";

    const captionContent = document.createElement('div');
    captionContent.className = "row";

    const colLeft = document.createElement('div');
    colLeft.className = "col-6";
    let fromItemRow = (tableConfig.itemsPerPage * tableConfig.currentPage) - tableConfig.itemsPerPage + 1;
    let toItemRow = tableConfig.itemsPerPage * tableConfig.currentPage;
    const showingText = document.createTextNode(`Showing ${fromItemRow} to ${serialNo} of ${tableConfig.totalItems} entries`);
    colLeft.appendChild(showingText);
    captionContent.appendChild(colLeft);

    const colRight = document.createElement('div');
    colRight.className = "col-6";

    const nav = document.createElement('nav');
    nav.setAttribute('aria-label', 'Page navigation');
    nav.className = "pagination justify-content-end";

    const prevLi = document.createElement('li');
    prevLi.className = "page-item prev";
    const prevLink = document.createElement('a');
    prevLink.className = "page-link";
    prevLink.href = "javascript:void(0);";
    prevLink.textContent = "Previous"; // Set text content for the "Previous" button
    prevLink.id = "prevButton"; // Add an ID to the "Previous" button
    prevLi.appendChild(prevLink);
    nav.appendChild(prevLi);

    const currentPageLi = document.createElement('li');
    currentPageLi.className = "page-item active";
    const currentPageLink = document.createElement('a');
    currentPageLink.className = "page-link ";
    currentPageLink.href = "javascript:void(0);";
    currentPageLink.textContent = currentPage; // Display current page number
    currentPageLi.appendChild(currentPageLink);
    nav.appendChild(currentPageLi);

    const nextLi = document.createElement('li');
    nextLi.className = "page-item next";
    const nextLink = document.createElement('a');
    nextLink.className = "page-link";
    nextLink.href = "javascript:void(0);";
    nextLink.textContent = "Next"; // Set text content for the "Next" button
    nextLink.id = "nextButton"; // Add an ID to the "Next" button
    nextLi.appendChild(nextLink);
    nav.appendChild(nextLi);
    colRight.appendChild(nav);
    captionContent.appendChild(colRight);
    caption.appendChild(captionContent);
    table.appendChild(caption);

    prevLink.addEventListener('click', async () => {
        //updateTable();

        if (currentPage > 1) {
            currentPage--;
            updateTable();
        }
        else {
            showToast("", "Warning", "You are in page one.");
        }
    });

    nextLink.addEventListener('click', async () => {
        //updateTable();

        if (currentPage < totalPages) {
            currentPage++;
            updateTable();
        }
        else {
            showToast("", "Warning", "No more page left.");
        }
    });

    rowDiv.appendChild(tableDiv);

    async function updateTable() {
        currentPageLink.textContent = currentPage; // Display current page number

        let tableInfo = {
            itemsPerPage: itemsPerPage,
            searchText: searchText,
            currentPage: currentPage,
        }
        tableConfig.filterAction(tableInfo);
    }
    //updateTable();
    return rowDiv;
}


function getBooleanElement(obj, key) {
    let statusName = document.createElement('span');
    switch (obj[key]) {
        case true:
            statusName.className = "badge bg-label-primary me-1";
            statusName.innerHTML = "True";
            break;
        case false:
            statusName.className = "badge bg-label-danger me-1";
            statusName.innerHTML = "False";
            break;
        default:
            statusName.innerHTML = "";
    }
    return statusName;
}

function getStatusElement(obj, key) {
    let statusName = document.createElement('span');
    switch (obj[key]) {
        case 1:
            statusName.className = "badge bg-label-primary me-1";
            statusName.innerHTML = "IsActive";
            break;
        case 0:
            statusName.className = "badge bg-label-danger me-1";
            statusName.innerHTML = "InActive";
            break;
        default:
            statusName.innerHTML = "";
    }
    return statusName;
}

function createActionButton(title, iconClass, buttonClass, onClick) {
    let button = document.createElement('button');
    button.title = title;
    button.type = 'button';
    button.className = `btn btn-icon ${buttonClass}`;
    button.addEventListener('click', onClick);

    let icon = document.createElement('span');
    icon.className = `tf-icons bx ${iconClass}`;
    button.appendChild(icon);
    return button;
}

function createSelectFieldWithLabel({ columnClass, id, labelText, options, defaultOptionText = 'Default Select', defaultOptionValue = '', isRequired = false }) {
    if (!columnClass || !id || !labelText || !options) {
        throw new Error('createSelectFieldWithLabel requires an object parameter with columnClass, id, labelText, and options properties');
    }

    const container = document.createElement('div');
    container.className = columnClass;

    const label = document.createElement('label');
    label.htmlFor = id;
    label.className = 'form-label';
    label.textContent = labelText;

    const selectElement = document.createElement('select');
    selectElement.id = id;
    selectElement.className = 'form-select';

    const defaultOption = document.createElement('option');
    defaultOption.value = defaultOptionValue;
    defaultOption.textContent = defaultOptionText;
    selectElement.appendChild(defaultOption);

    container.appendChild(label);
    container.appendChild(selectElement);
    const invalidFeedbackElement = document.createElement('div');
    if (isRequired) {
        label.classList.add('asterisk');
        selectElement.required = true;

        invalidFeedbackElement.classList.add('invalid-feedback');
        invalidFeedbackElement.textContent = `${labelText} is required!`;
        container.appendChild(invalidFeedbackElement);
    }
    options.forEach(({ id, name }) => {
        const option = document.createElement('option');
        option.value = id;
        option.textContent = name;
        selectElement.appendChild(option);
    });

    return {
        id,
        selectElement,
        invalidFeedbackElement,
        setValue: (value) => {
            selectElement.value = value;
        },
        getValue: () => {
            return selectElement.value;
        },
        div: container
    };
}

function createInputFieldWithLabel({ columnClass, type, id, labelText, placeholder, isReadOnly = false, isRequired = false }) {
    if (!columnClass || !type || !id || !labelText || !placeholder) {
        throw new Error('createInputFieldWithLabel requires an object parameter with columnClass, type, id, labelText, and placeholder properties');
    }

    const container = document.createElement('div');
    container.className = "mb-1 " + columnClass;

    const label = document.createElement('label');
    label.htmlFor = id;
    label.className = 'form-label';
    label.textContent = labelText;

    const inputElement = document.createElement('input');
    inputElement.type = type;
    inputElement.id = id;
    inputElement.className = 'form-control';
    inputElement.placeholder = placeholder;
    if (type === "number") {
        inputElement.step = "any";
    }
    container.appendChild(label);
    container.appendChild(inputElement);
    const invalidFeedbackElement = document.createElement('div');

    if (isRequired) {
        label.classList.add('asterisk');
        inputElement.required = true;
        invalidFeedbackElement.classList.add('invalid-feedback');
        invalidFeedbackElement.textContent = `${labelText} is required!`;
        container.appendChild(invalidFeedbackElement);
    };
    if (isReadOnly) {
        inputElement.readOnly = true;
    }

    return {
        id,
        inputElement,
        invalidFeedbackElement,
        setValue: (value) => {
            inputElement.value = value;
        },
        getValue: () => {
            return inputElement.value;
        },
        div: container
    };
}


function createModalContentWithForm({ modalTitle, btnClassName, buttonName }) {

    const form = document.createElement('form');
    form.classList.add('modal-content', 'needs-validation', 'form');
    form.setAttribute('novalidate', '');

    const header = document.createElement('div');
    header.classList.add('modal-header');

    const title = document.createElement('h5');
    title.classList.add('modal-title');
    title.setAttribute('id', 'backDropModalTitle');
    title.textContent = modalTitle;

    const closeButton = document.createElement('button');
    closeButton.classList.add('btn-close');
    closeButton.setAttribute('type', 'button');
    closeButton.setAttribute('data-bs-dismiss', 'modal');
    closeButton.setAttribute('aria-label', 'Close');

    header.appendChild(title);
    header.appendChild(closeButton);

    const body = document.createElement('div');
    body.classList.add('modal-body');

    const row = document.createElement('div');
    row.classList.add('row');

    body.appendChild(row);

    const footer = document.createElement('div');
    footer.classList.add('modal-footer');

    const closeButton2 = document.createElement('button');
    closeButton2.classList.add('btn', 'btn-outline-secondary');
    closeButton2.setAttribute('type', 'button');
    closeButton2.setAttribute('data-bs-dismiss', 'modal');
    closeButton2.textContent = 'Close';

    const saveButton = document.createElement('button');
    saveButton.classList.add('btn', btnClassName);
    saveButton.setAttribute('type', 'submit');
    saveButton.textContent = buttonName;

    footer.appendChild(closeButton2);
    footer.appendChild(saveButton);

    form.appendChild(header);
    form.appendChild(body);
    form.appendChild(footer);

    return {
        modal_body: row,
        form: form
    };
};


class ModalFormHandler {
    constructor(modalConfig) {
        this.modalConfig = modalConfig;
        this.modal = new bootstrap.Modal(document.getElementById(modalConfig.modalId));
    }
    createForm(formFields) {
        const modalDialog = document.querySelector(".modal-dialog");
        modalDialog.innerHTML = "";

        const content = createModalContentWithForm({
            modalTitle: this.modalConfig.modalTitle,
            btnClassName: this.modalConfig.btnClassName,
            buttonName: this.modalConfig.buttonName
        });
        if (formFields) {

            formFields.forEach(elements => {
                content.modal_body.appendChild(elements.div);
            });
        }

        modalDialog.appendChild(content.form);

        this.modal.show();

        const submitFunction = async (onSubmitCallback) => {
            content.form.addEventListener('submit', async (e) => {
                e.preventDefault();
                if (!content.form.checkValidity()) {
                    e.stopPropagation();
                    content.form.classList.add('was-validated');
                } else {
                    const formData = {};
                    if (formFields) {

                        formFields.forEach(elements => {
                            formData[elements.id] = elements.getValue();
                        });
                    }

                    await onSubmitCallback(formData, this.modal); // Pass modal reference
                }
            });
        };

        return {
            submit: submitFunction
        };
    }
}
//
async function createForm({ btnClassName, buttonName }) {

    // Create the form element
    const form = document.createElement('form');
    form.classList.add('needs-validation', 'form');
    form.setAttribute('novalidate', '');


    const row = document.createElement('div');
    row.classList.add('row');

    form.appendChild(row);

    // Create the footer section of the form
    const footer = document.createElement('div');
    footer.classList.add('text-end', 'mt-1');

    const saveButton = document.createElement('button');
    saveButton.className = "btn " + btnClassName;
    saveButton.setAttribute('type', 'submit');
    saveButton.textContent = buttonName;

    footer.appendChild(saveButton);

    // Add the sections to the form
    form.appendChild(footer);

    return {
        row: row,
        form: form
    };
}