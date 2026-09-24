<template>
  <div class="workspace">
    <aside class="sidebar">
      <div class="brand">
        <span class="brand-mark">K</span>
        <div>
          <strong>Kier Records</strong>
          <small>Financial records system</small>
        </div>
      </div>

      <nav class="nav-list" aria-label="Main sections">
        <button
          v-for="item in navItems"
          :key="item.id"
          type="button"
          :class="{ active: activeView === item.id }"
          @click="setView(item.id)"
        >
          <span>{{ item.short }}</span>
          {{ item.label }}
        </button>
      </nav>

      <section class="sidebar-summary" aria-label="Current balance">
        <span>Available Funds</span>
        <strong>{{ money(availableFunds) }}</strong>
        <small>{{ collections.length }} receipts, {{ disbursements.length }} expenses</small>
      </section>

      <div class="api-card">
        <span class="status-dot" :class="{ online: apiOnline }"></span>
        <div>
          <strong>{{ apiOnline ? 'Backend online' : 'Frontend mode' }}</strong>
          <small>{{ healthDetail }}</small>
        </div>
      </div>
    </aside>

    <main class="content">
      <header class="topbar">
        <div>
          <p class="eyebrow">{{ activeSection.eyebrow }}</p>
          <h1>{{ activeSection.title }}</h1>
        </div>

        <div class="toolbar">
          <label class="search">
            <span>Search</span>
            <input v-model="searchTerm" type="search" placeholder="Name, ID, receipt, event..." />
          </label>
          <button type="button" class="secondary-action" @click="exportCsv">Export CSV</button>
          <button type="button" class="primary-action" @click="primaryAction">{{ activeSection.action }}</button>
        </div>
      </header>

      <p v-if="toastMessage" class="toast">{{ toastMessage }}</p>

      <section v-if="activeView === 'dashboard'" class="view-stack">
        <div class="stat-grid">
          <article v-for="stat in stats" :key="stat.label" class="stat-card">
            <span>{{ stat.label }}</span>
            <strong>{{ stat.value }}</strong>
            <small>{{ stat.detail }}</small>
          </article>
        </div>

        <div class="split-grid">
          <section class="panel">
            <div class="panel-heading">
              <div>
                <h2>Monthly Cash Flow</h2>
                <span>Collections against disbursements</span>
              </div>
            </div>
            <div class="chart-list">
              <div v-for="item in cashFlow" :key="item.month" class="chart-row">
                <span>{{ item.month }}</span>
                <div class="bar-track">
                  <span class="bar income" :style="{ width: `${item.incomeWidth}%` }"></span>
                  <span class="bar expense" :style="{ width: `${item.expenseWidth}%` }"></span>
                </div>
                <strong>{{ money(item.net) }}</strong>
              </div>
            </div>
          </section>

          <section class="panel">
            <div class="panel-heading">
              <div>
                <h2>Recent Activity</h2>
                <span>Latest changes in this workspace</span>
              </div>
            </div>
            <ul class="activity-list">
              <li v-for="item in activity.slice(0, 7)" :key="item.id">
                <span>{{ item.type }}</span>
                <div>
                  <strong>{{ item.title }}</strong>
                  <small>{{ item.detail }}</small>
                </div>
              </li>
            </ul>
          </section>
        </div>

        <section class="panel">
          <div class="panel-heading">
            <div>
              <h2>Priority Students</h2>
              <span>Students with balances or recent attendance concerns</span>
            </div>
            <button type="button" @click="setView('students')">Open Directory</button>
          </div>
          <table>
            <thead>
              <tr>
                <th>Student</th>
                <th>Course</th>
                <th>Balance</th>
                <th>Attendance</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="student in priorityStudents" :key="student.id" @click="selectStudent(student.id)">
                <td>{{ student.studentNo }} - {{ student.name }}</td>
                <td>{{ student.course }} {{ student.section }}</td>
                <td>{{ money(balanceFor(student.id)) }}</td>
                <td>{{ attendanceFor(student.id) }}%</td>
              </tr>
            </tbody>
          </table>
        </section>
      </section>

      <section v-else-if="activeView === 'students'" class="data-layout">
        <form class="panel form-panel" @submit.prevent="saveStudent">
          <div class="form-title">
            <h2>{{ editingStudentId ? 'Edit Student' : 'Add Student' }}</h2>
            <button v-if="editingStudentId" type="button" class="text-action" @click="resetStudentForm">Cancel</button>
          </div>
          <label>
            Student ID
            <input v-model="studentForm.studentNo" type="text" required />
          </label>
          <label>
            Full Name
            <input v-model="studentForm.name" type="text" required />
          </label>
          <label>
            Course
            <input v-model="studentForm.course" type="text" required />
          </label>
          <label>
            Section
            <input v-model="studentForm.section" type="text" required />
          </label>
          <label>
            Contact
            <input v-model="studentForm.contact" type="text" />
          </label>
          <label>
            RFID UID
            <input v-model="studentForm.rfidUid" type="text" placeholder="Tap card or type UID" />
          </label>
          <button type="submit" class="primary-action">{{ editingStudentId ? 'Update Student' : 'Save Student' }}</button>
        </form>

        <section class="panel">
          <div class="panel-heading">
            <div>
              <h2>Student Directory</h2>
              <span>{{ filteredStudents.length }} records</span>
            </div>
          </div>
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Course</th>
                <th>RFID</th>
                <th>Balance</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="student in filteredStudents" :key="student.id">
                <td>{{ student.studentNo }}</td>
                <td>
                  <button type="button" class="row-link" @click="selectStudent(student.id)">{{ student.name }}</button>
                </td>
                <td>{{ student.course }} {{ student.section }}</td>
                <td>{{ student.rfidUid || 'Not mapped' }}</td>
                <td>{{ money(balanceFor(student.id)) }}</td>
                <td class="table-actions">
                  <button type="button" @click="editStudent(student)">Edit</button>
                  <button type="button" @click="removeStudent(student.id)">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </section>
      </section>

      <section v-else-if="activeView === 'collections'" class="data-layout">
        <form class="panel form-panel" @submit.prevent="addCollection">
          <h2>Add Collection</h2>
          <label>
            Student
            <select v-model.number="collectionForm.studentId">
              <option v-for="student in students" :key="student.id" :value="student.id">{{ student.name }}</option>
            </select>
          </label>
          <label>
            Category
            <select v-model="collectionForm.category">
              <option>Department Fee</option>
              <option>Event Contribution</option>
              <option>Fine Payment</option>
              <option>Fundraising</option>
            </select>
          </label>
          <label>
            Amount
            <input v-model.number="collectionForm.amount" type="number" min="1" step="1" />
          </label>
          <label>
            Receipt
            <input v-model="collectionForm.receipt" type="text" />
          </label>
          <button type="submit" class="primary-action">Save Collection</button>
        </form>

        <section class="panel">
          <div class="panel-heading">
            <div>
              <h2>Collections Ledger</h2>
              <span>{{ filteredCollections.length }} visible receipts</span>
            </div>
            <strong>{{ money(totalCollections) }}</strong>
          </div>
          <table>
            <thead>
              <tr>
                <th>Receipt</th>
                <th>Student</th>
                <th>Category</th>
                <th>Amount</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="collection in filteredCollections" :key="collection.id">
                <td>{{ collection.receipt }}</td>
                <td>{{ studentName(collection.studentId) }}</td>
                <td>{{ collection.category }}</td>
                <td>{{ money(collection.amount) }}</td>
                <td class="table-actions">
                  <button type="button" @click="removeCollection(collection.id)">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </section>
      </section>

      <section v-else-if="activeView === 'fines'" class="data-layout">
        <form class="panel form-panel" @submit.prevent="addFine">
          <h2>Add Fine</h2>
          <label>
            Student
            <select v-model.number="fineForm.studentId">
              <option v-for="student in students" :key="student.id" :value="student.id">{{ student.name }}</option>
            </select>
          </label>
          <label>
            Category
            <input v-model="fineForm.category" type="text" required />
          </label>
          <label>
            Amount
            <input v-model.number="fineForm.amount" type="number" min="1" step="1" />
          </label>
          <label>
            Status
            <select v-model="fineForm.status">
              <option>Unpaid</option>
              <option>Paid</option>
            </select>
          </label>
          <button type="submit" class="primary-action">Save Fine</button>
        </form>

        <section class="panel">
          <div class="panel-heading">
            <div>
              <h2>Fines Register</h2>
              <span>{{ filteredFines.length }} visible fines</span>
            </div>
            <strong>{{ money(unpaidFines) }} unpaid</strong>
          </div>
          <table>
            <thead>
              <tr>
                <th>Student</th>
                <th>Category</th>
                <th>Amount</th>
                <th>Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="fine in filteredFines" :key="fine.id">
                <td>{{ studentName(fine.studentId) }}</td>
                <td>{{ fine.category }}</td>
                <td>{{ money(fine.amount) }}</td>
                <td><span class="badge" :class="{ paid: fine.status === 'Paid' }">{{ fine.status }}</span></td>
                <td class="table-actions">
                  <button type="button" @click="toggleFine(fine.id)">{{ fine.status === 'Paid' ? 'Unpay' : 'Pay' }}</button>
                  <button type="button" @click="removeFine(fine.id)">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </section>
      </section>

      <section v-else-if="activeView === 'attendance'" class="view-stack">
        <section class="qr-scanner-grid">
          <form class="panel form-panel" @submit.prevent="addAttendance">
            <h2>Manual Attendance</h2>
            <label>
              Event
              <input v-model="attendanceForm.event" type="text" required />
            </label>
            <label>
              Student
              <select v-model.number="attendanceForm.studentId">
                <option v-for="student in students" :key="student.id" :value="student.id">{{ student.name }}</option>
              </select>
            </label>
            <label>
              Status
              <select v-model="attendanceForm.status">
                <option>Present</option>
                <option>Late</option>
                <option>Absent</option>
                <option>Excused</option>
              </select>
            </label>
            <button type="submit" class="primary-action">Save Attendance</button>
          </form>

          <section class="panel scanner-panel">
            <div class="panel-heading">
              <div>
                <h2>QR / RFID Scanner</h2>
                <span>Scan a QR code or tap an RFID card reader</span>
              </div>
              <button type="button" @click="scannerActive ? stopQrScanner() : startQrScanner()">
                {{ scannerActive ? 'Stop Camera' : 'Start Camera' }}
              </button>
            </div>
            <div class="scanner-body">
              <video ref="scannerVideo" class="scanner-video" autoplay muted playsinline></video>
              <div class="scanner-controls">
                <label>
                  Event
                  <input v-model="scanForm.eventTitle" type="text" />
                </label>
                <label>
                  Status
                  <select v-model="scanForm.status">
                    <option>Present</option>
                    <option>Late</option>
                    <option>Absent</option>
                    <option>Excused</option>
                  </select>
                </label>
                <label>
                  Manual QR / Student ID
                  <input v-model="manualQr" type="text" placeholder="KIER:2026-001 or 2026-001" />
                </label>
                <button type="button" class="primary-action" @click="recordQrScan(manualQr)">Record Scan</button>
                <label>
                  RFID Reader
                  <input
                    v-model="manualRfid"
                    type="text"
                    placeholder="Focus here, then tap RFID card"
                    @keyup.enter="recordRfidScan(manualRfid)"
                  />
                </label>
                <button type="button" class="secondary-action" @click="recordRfidScan(manualRfid)">Record RFID</button>
              </div>
              <p class="scanner-message">{{ scannerMessage }}</p>
            </div>
          </section>
        </section>

        <section class="panel">
          <div class="panel-heading">
            <div>
              <h2>Attendance Records</h2>
              <span>{{ filteredAttendance.length }} visible entries</span>
            </div>
            <strong>{{ attendanceRate }}%</strong>
          </div>
          <table>
            <thead>
              <tr>
                <th>Event</th>
                <th>Student</th>
                <th>Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="record in filteredAttendance" :key="record.id">
                <td>{{ record.event }}</td>
                <td>{{ studentName(record.studentId) }}</td>
                <td><span class="badge neutral">{{ record.status }}</span></td>
                <td class="table-actions">
                  <button type="button" @click="removeAttendance(record.id)">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </section>
      </section>

      <section v-else-if="activeView === 'reports'" class="view-stack">
        <div class="stat-grid">
          <article class="stat-card">
            <span>Total Collected</span>
            <strong>{{ money(totalCollections) }}</strong>
            <small>All receipts in the ledger</small>
          </article>
          <article class="stat-card">
            <span>Expenses</span>
            <strong>{{ money(totalDisbursements) }}</strong>
            <small>Department disbursements</small>
          </article>
          <article class="stat-card">
            <span>Unpaid Fines</span>
            <strong>{{ money(unpaidFines) }}</strong>
            <small>Outstanding balances</small>
          </article>
          <article class="stat-card">
            <span>Attendance Rate</span>
            <strong>{{ attendanceRate }}%</strong>
            <small>Present or late records</small>
          </article>
        </div>

        <div class="split-grid">
          <section class="panel report-panel">
            <div class="panel-heading flat">
              <div>
                <h2>Report Summary</h2>
                <span>Ready for printing or CSV export</span>
              </div>
            </div>
            <dl class="report-list">
              <div>
                <dt>Active students</dt>
                <dd>{{ students.length }}</dd>
              </div>
              <div>
                <dt>Receipts</dt>
                <dd>{{ collections.length }}</dd>
              </div>
              <div>
                <dt>Fine records</dt>
                <dd>{{ fines.length }}</dd>
              </div>
              <div>
                <dt>Attendance entries</dt>
                <dd>{{ attendanceRecords.length }}</dd>
              </div>
            </dl>
            <div class="button-row">
              <button type="button" class="primary-action" @click="windowPrint">Print Report</button>
              <button type="button" class="secondary-action" @click="resetDemoData">Reset Demo Data</button>
            </div>
          </section>

          <form class="panel form-panel" @submit.prevent="addDisbursement">
            <h2>Add Expense</h2>
            <label>
              Description
              <input v-model="disbursementForm.description" type="text" required />
            </label>
            <label>
              Amount
              <input v-model.number="disbursementForm.amount" type="number" min="1" step="1" />
            </label>
            <button type="submit" class="primary-action">Save Expense</button>
          </form>
        </div>

        <section class="panel">
          <div class="panel-heading">
            <div>
              <h2>Disbursements</h2>
              <span>{{ disbursements.length }} expenses</span>
            </div>
          </div>
          <table>
            <thead>
              <tr>
                <th>Description</th>
                <th>Amount</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in disbursements" :key="item.id">
                <td>{{ item.description }}</td>
                <td>{{ money(item.amount) }}</td>
                <td class="table-actions">
                  <button type="button" @click="removeDisbursement(item.id)">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </section>
      </section>

      <aside v-if="selectedStudent" class="student-drawer">
        <button type="button" class="drawer-close" @click="selectedStudentId = null">Close</button>
        <p class="eyebrow">Student Profile</p>
        <h2>{{ selectedStudent.name }}</h2>
        <p>{{ selectedStudent.studentNo }} - {{ selectedStudent.course }} {{ selectedStudent.section }}</p>
        <figure class="qr-card">
          <img v-if="studentQrCodes[selectedStudent.studentNo]" :src="studentQrCodes[selectedStudent.studentNo]" :alt="`QR code for ${selectedStudent.name}`" />
          <figcaption>{{ qrPayload(selectedStudent.studentNo) }}</figcaption>
        </figure>
        <div class="mini-stats">
          <span>Balance <strong>{{ money(balanceFor(selectedStudent.id)) }}</strong></span>
          <span>Attendance <strong>{{ attendanceFor(selectedStudent.id) }}%</strong></span>
          <span>Contact <strong>{{ selectedStudent.contact || 'No contact' }}</strong></span>
          <span>RFID <strong>{{ selectedStudent.rfidUid || 'Not mapped' }}</strong></span>
        </div>
      </aside>
    </main>
  </div>
</template>

<script setup>
import QRCode from 'qrcode';
import { computed, onBeforeUnmount, onMounted, reactive, ref, watch } from 'vue';

const storageKey = 'kier-records-v2';

const demoData = {
  students: [
    { id: 1, studentNo: '2026-001', name: 'Mika Reyes', course: 'BSIT', section: '3A', contact: '0917 100 1101', rfidUid: 'RFID2026001' },
    { id: 2, studentNo: '2026-014', name: 'Aaron Cruz', course: 'BSCS', section: '2B', contact: '0918 200 2202', rfidUid: 'RFID2026014' },
    { id: 3, studentNo: '2026-027', name: 'Lia Santos', course: 'BSIS', section: '4A', contact: '0919 300 3303', rfidUid: 'RFID2026027' },
    { id: 4, studentNo: '2026-035', name: 'Noah Dela Cruz', course: 'BSIT', section: '1C', contact: '0920 400 4404', rfidUid: 'RFID2026035' },
  ],
  collections: [
    { id: 1, receipt: 'OR-1001', studentId: 1, category: 'Department Fee', amount: 750, month: 'Aug' },
    { id: 2, receipt: 'OR-1002', studentId: 2, category: 'Event Contribution', amount: 350, month: 'Aug' },
    { id: 3, receipt: 'OR-1003', studentId: 3, category: 'Fine Payment', amount: 120, month: 'Sep' },
    { id: 4, receipt: 'OR-1004', studentId: 4, category: 'Fundraising', amount: 500, month: 'Sep' },
  ],
  fines: [
    { id: 1, studentId: 2, category: 'Late attendance', amount: 50, status: 'Unpaid' },
    { id: 2, studentId: 4, category: 'Missed meeting', amount: 100, status: 'Unpaid' },
    { id: 3, studentId: 3, category: 'Uniform violation', amount: 120, status: 'Paid' },
  ],
  attendanceRecords: [
    { id: 1, event: 'General Assembly', studentId: 1, status: 'Present' },
    { id: 2, event: 'General Assembly', studentId: 2, status: 'Late' },
    { id: 3, event: 'General Assembly', studentId: 3, status: 'Present' },
    { id: 4, event: 'Clean-up Drive', studentId: 4, status: 'Absent' },
    { id: 5, event: 'Clean-up Drive', studentId: 1, status: 'Present' },
  ],
  disbursements: [
    { id: 1, description: 'Meeting supplies', amount: 280, month: 'Aug' },
    { id: 2, description: 'Event materials', amount: 430, month: 'Sep' },
  ],
  activity: [
    { id: 1, type: 'Receipt', title: 'OR-1004 recorded', detail: 'Fundraising payment added' },
    { id: 2, type: 'Fine', title: 'Missed meeting fine', detail: 'Noah Dela Cruz has an unpaid fine' },
    { id: 3, type: 'Attendance', title: 'Clean-up Drive recorded', detail: 'Attendance entry added' },
  ],
};

const activeView = ref('dashboard');
const searchTerm = ref('');
const selectedStudentId = ref(null);
const editingStudentId = ref(null);
const toastMessage = ref('');
const health = ref(null);
const healthError = ref('');
const scannerVideo = ref(null);
const scannerActive = ref(false);
const scannerMessage = ref('Camera scanner is ready. You can also type the QR value manually.');
const manualQr = ref('');
const manualRfid = ref('');
const studentQrCodes = ref({});
let scannerStream = null;
let scannerTimer = null;
let lastScan = { value: '', time: 0 };

const students = ref(copy(demoData.students));
const collections = ref(copy(demoData.collections));
const fines = ref(copy(demoData.fines));
const attendanceRecords = ref(copy(demoData.attendanceRecords));
const disbursements = ref(copy(demoData.disbursements));
const activity = ref(copy(demoData.activity));

const navItems = [
  { id: 'dashboard', label: 'Dashboard', short: 'DB' },
  { id: 'students', label: 'Students', short: 'ST' },
  { id: 'collections', label: 'Collections', short: 'CO' },
  { id: 'fines', label: 'Fines', short: 'FI' },
  { id: 'attendance', label: 'Attendance', short: 'AT' },
  { id: 'reports', label: 'Reports', short: 'RP' },
];

const sections = {
  dashboard: { eyebrow: 'Overview', title: 'Department records dashboard', action: 'Add Receipt' },
  students: { eyebrow: 'Directory', title: 'Students and balances', action: 'Add Student' },
  collections: { eyebrow: 'Ledger', title: 'Collections and receipts', action: 'Add Receipt' },
  fines: { eyebrow: 'Register', title: 'Fines and payment status', action: 'Add Fine' },
  attendance: { eyebrow: 'Events', title: 'Attendance monitoring', action: 'Record Attendance' },
  reports: { eyebrow: 'Summary', title: 'Financial and activity reports', action: 'Print' },
};

const studentForm = reactive(blankStudent());
const collectionForm = reactive({ studentId: 1, category: 'Department Fee', amount: 100, receipt: 'OR-1005' });
const fineForm = reactive({ studentId: 1, category: 'Late attendance', amount: 50, status: 'Unpaid' });
const attendanceForm = reactive({ event: 'General Assembly', studentId: 1, status: 'Present' });
const scanForm = reactive({ eventTitle: 'General Assembly', status: 'Present' });
const disbursementForm = reactive({ description: 'Department expense', amount: 100 });

const activeSection = computed(() => sections[activeView.value]);
const apiOnline = computed(() => ['ok', 'running'].includes(String(health.value?.status).toLowerCase()));
const healthDetail = computed(() => {
  if (health.value) {
    return `Database: ${health.value.database || 'Connected'}`;
  }

  return healthError.value || 'Saved locally in this browser';
});
const selectedStudent = computed(() => students.value.find((student) => student.id === selectedStudentId.value));
const normalizedSearch = computed(() => searchTerm.value.trim().toLowerCase());
const totalCollections = computed(() => collections.value.reduce((sum, item) => sum + Number(item.amount), 0));
const totalDisbursements = computed(() => disbursements.value.reduce((sum, item) => sum + Number(item.amount), 0));
const availableFunds = computed(() => totalCollections.value - totalDisbursements.value);
const unpaidFines = computed(() =>
  fines.value.filter((fine) => fine.status !== 'Paid').reduce((sum, item) => sum + Number(item.amount), 0),
);
const attendanceRate = computed(() => {
  if (!attendanceRecords.value.length) {
    return 0;
  }

  const counted = attendanceRecords.value.filter((record) => ['Present', 'Late'].includes(record.status)).length;
  return Math.round((counted / attendanceRecords.value.length) * 100);
});
const stats = computed(() => [
  { label: 'Available Funds', value: money(availableFunds.value), detail: 'Collections minus expenses' },
  { label: 'Collected', value: money(totalCollections.value), detail: `${collections.value.length} receipts` },
  { label: 'Unpaid Fines', value: money(unpaidFines.value), detail: `${fines.value.filter((fine) => fine.status !== 'Paid').length} open fines` },
  { label: 'Attendance', value: `${attendanceRate.value}%`, detail: 'Present or late records' },
]);
const priorityStudents = computed(() =>
  students.value
    .map((student) => ({
      ...student,
      balance: balanceFor(student.id),
      attendance: attendanceFor(student.id),
    }))
    .sort((a, b) => b.balance - a.balance || a.attendance - b.attendance)
    .slice(0, 5),
);
const cashFlow = computed(() => {
  const months = ['Aug', 'Sep', 'Oct', 'Nov'];
  const max = Math.max(totalCollections.value, totalDisbursements.value, 1);

  return months.map((month) => {
    const income = collections.value.filter((item) => item.month === month).reduce((sum, item) => sum + Number(item.amount), 0);
    const expense = disbursements.value.filter((item) => item.month === month).reduce((sum, item) => sum + Number(item.amount), 0);

    return {
      month,
      net: income - expense,
      incomeWidth: Math.max(4, Math.round((income / max) * 100)),
      expenseWidth: Math.max(4, Math.round((expense / max) * 100)),
    };
  });
});
const filteredStudents = computed(() => filterBy(students.value, (student) => [student.studentNo, student.name, student.course, student.section]));
const filteredCollections = computed(() =>
  filterBy(collections.value, (collection) => [collection.receipt, collection.category, studentName(collection.studentId)]),
);
const filteredFines = computed(() => filterBy(fines.value, (fine) => [fine.category, fine.status, studentName(fine.studentId)]));
const filteredAttendance = computed(() =>
  filterBy(attendanceRecords.value, (record) => [record.event, record.status, studentName(record.studentId)]),
);

watch([students, collections, fines, attendanceRecords, disbursements, activity], saveState, { deep: true });
watch(students, generateStudentQrCodes, { deep: true });

onMounted(async () => {
  loadState();
  await generateStudentQrCodes();

  try {
    const response = await fetch('/api/health');
    if (!response.ok) {
      throw new Error(`Health check failed with ${response.status}`);
    }

    health.value = await response.json();
  } catch (error) {
    healthError.value = error instanceof Error ? error.message : 'Backend not reachable';
  }
});

onBeforeUnmount(() => {
  stopQrScanner();
});

function copy(value) {
  return JSON.parse(JSON.stringify(value));
}

function blankStudent() {
  return { studentNo: '', name: '', course: 'BSIT', section: '1A', contact: '', rfidUid: '' };
}

function setView(view) {
  activeView.value = view;
  searchTerm.value = '';
}

function selectStudent(studentId) {
  selectedStudentId.value = studentId;
}

function qrPayload(studentNo) {
  return `KIER:${studentNo}`;
}

function parseQrPayload(value) {
  const cleaned = String(value || '').trim();
  if (!cleaned) {
    return '';
  }

  return cleaned.toUpperCase().startsWith('KIER:') ? cleaned.slice(5).trim() : cleaned;
}

function normalizeRfid(value) {
  return String(value || '').trim().replaceAll(' ', '').toUpperCase();
}

async function generateStudentQrCodes() {
  const nextCodes = {};

  for (const student of students.value) {
    nextCodes[student.studentNo] = await QRCode.toDataURL(qrPayload(student.studentNo), {
      width: 240,
      margin: 2,
      color: {
        dark: '#142027',
        light: '#ffffff',
      },
    });
  }

  studentQrCodes.value = nextCodes;
}

function filterBy(records, valuesFor) {
  if (!normalizedSearch.value) {
    return records;
  }

  return records.filter((record) =>
    valuesFor(record).some((value) => String(value).toLowerCase().includes(normalizedSearch.value)),
  );
}

function studentName(studentId) {
  return students.value.find((student) => student.id === studentId)?.name || 'Unknown student';
}

function balanceFor(studentId) {
  const openFines = fines.value
    .filter((fine) => fine.studentId === studentId && fine.status !== 'Paid')
    .reduce((sum, fine) => sum + Number(fine.amount), 0);
  const finePayments = collections.value
    .filter((collection) => collection.studentId === studentId && collection.category === 'Fine Payment')
    .reduce((sum, collection) => sum + Number(collection.amount), 0);

  return Math.max(openFines - finePayments, 0);
}

function attendanceFor(studentId) {
  const records = attendanceRecords.value.filter((record) => record.studentId === studentId);
  if (!records.length) {
    return 0;
  }

  const counted = records.filter((record) => ['Present', 'Late'].includes(record.status)).length;
  return Math.round((counted / records.length) * 100);
}

function money(value) {
  return new Intl.NumberFormat('en-PH', {
    style: 'currency',
    currency: 'PHP',
    maximumFractionDigits: 0,
  }).format(value);
}

function nextId(records) {
  return Math.max(0, ...records.map((item) => Number(item.id))) + 1;
}

function nextReceipt() {
  return `OR-${1001 + collections.value.length}`;
}

function currentMonth() {
  return new Date().toLocaleString('en-US', { month: 'short' });
}

function notify(message) {
  toastMessage.value = message;
  window.clearTimeout(notify.timer);
  notify.timer = window.setTimeout(() => {
    toastMessage.value = '';
  }, 2400);
}

function logActivity(type, title, detail) {
  activity.value.unshift({ id: Date.now(), type, title, detail });
}

async function saveStudent() {
  if (editingStudentId.value) {
    const index = students.value.findIndex((student) => student.id === editingStudentId.value);
    if (index >= 0) {
      students.value[index] = { ...students.value[index], ...studentForm };
      logActivity('Student', `${studentForm.name} updated`, 'Student profile was changed');
      notify('Student updated');
    }
  } else {
    const student = { id: nextId(students.value), ...studentForm };
    students.value.unshift(student);
    await createBackendStudent(student);
    selectedStudentId.value = student.id;
    logActivity('Student', `${student.name} added`, 'New student record created');
    notify('Student saved');
  }

  resetStudentForm();
}

async function createBackendStudent(student) {
  const [firstName, ...lastNameParts] = student.name.trim().split(/\s+/);

  try {
    await fetch('/api/students', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        studentNo: student.studentNo,
        firstName: firstName || student.name,
        lastName: lastNameParts.join(' ') || '',
        course: student.course,
        section: student.section,
        contactNumber: student.contact || '',
        rfidUid: normalizeRfid(student.rfidUid),
      }),
    });
  } catch {
    // The frontend still works offline; QR scans will sync once the backend has the student.
  }
}

function editStudent(student) {
  editingStudentId.value = student.id;
  Object.assign(studentForm, {
    studentNo: student.studentNo,
    name: student.name,
    course: student.course,
    section: student.section,
    contact: student.contact || '',
    rfidUid: student.rfidUid || '',
  });
}

function resetStudentForm() {
  editingStudentId.value = null;
  Object.assign(studentForm, blankStudent());
}

function removeStudent(studentId) {
  students.value = students.value.filter((student) => student.id !== studentId);
  collections.value = collections.value.filter((collection) => collection.studentId !== studentId);
  fines.value = fines.value.filter((fine) => fine.studentId !== studentId);
  attendanceRecords.value = attendanceRecords.value.filter((record) => record.studentId !== studentId);
  if (selectedStudentId.value === studentId) {
    selectedStudentId.value = null;
  }
  logActivity('Student', 'Student removed', 'Related sample records were also removed');
  notify('Student deleted');
}

function addCollection() {
  const receipt = collectionForm.receipt || nextReceipt();
  collections.value.unshift({
    id: nextId(collections.value),
    receipt,
    studentId: collectionForm.studentId,
    category: collectionForm.category,
    amount: Number(collectionForm.amount || 0),
    month: currentMonth(),
  });
  logActivity('Receipt', `${receipt} recorded`, `${studentName(collectionForm.studentId)} paid ${money(collectionForm.amount)}`);
  collectionForm.amount = 100;
  collectionForm.receipt = nextReceipt();
  notify('Collection saved');
}

function removeCollection(collectionId) {
  collections.value = collections.value.filter((collection) => collection.id !== collectionId);
  logActivity('Receipt', 'Receipt removed', 'Collection entry deleted');
  notify('Collection deleted');
}

function addFine() {
  fines.value.unshift({
    id: nextId(fines.value),
    studentId: fineForm.studentId,
    category: fineForm.category,
    amount: Number(fineForm.amount || 0),
    status: fineForm.status,
  });
  logActivity('Fine', `${fineForm.category} fine added`, `${studentName(fineForm.studentId)} - ${money(fineForm.amount)}`);
  fineForm.amount = 50;
  notify('Fine saved');
}

function toggleFine(fineId) {
  const fine = fines.value.find((item) => item.id === fineId);
  if (!fine) {
    return;
  }

  fine.status = fine.status === 'Paid' ? 'Unpaid' : 'Paid';
  logActivity('Fine', `${fine.category} marked ${fine.status}`, studentName(fine.studentId));
  notify(`Fine marked ${fine.status.toLowerCase()}`);
}

function removeFine(fineId) {
  fines.value = fines.value.filter((fine) => fine.id !== fineId);
  logActivity('Fine', 'Fine removed', 'Fine record deleted');
  notify('Fine deleted');
}

async function addAttendance() {
  const student = students.value.find((item) => item.id === attendanceForm.studentId);
  if (student) {
    await recordQrScan(student.studentNo, { silent: true });
    return;
  }

  addAttendanceLocal(attendanceForm.event, attendanceForm.studentId, attendanceForm.status);
}

function addAttendanceLocal(event, studentId, status) {
  attendanceRecords.value.unshift({
    id: nextId(attendanceRecords.value),
    event,
    studentId,
    status,
  });
  logActivity('Attendance', `${event} recorded`, `${studentName(studentId)} - ${status}`);
  notify('Attendance saved');
}

async function startQrScanner() {
  if (!('BarcodeDetector' in window)) {
    scannerMessage.value = 'This browser does not support camera QR scanning. Type the QR value manually below.';
    return;
  }

  try {
    scannerStream = await navigator.mediaDevices.getUserMedia({
      video: { facingMode: { ideal: 'environment' } },
      audio: false,
    });
    scannerVideo.value.srcObject = scannerStream;
    scannerActive.value = true;
    scannerMessage.value = 'Camera is scanning. Point it at a student ID QR code.';

    const detector = new window.BarcodeDetector({ formats: ['qr_code'] });
    scannerTimer = window.setInterval(async () => {
      if (!scannerVideo.value || scannerVideo.value.readyState < 2) {
        return;
      }

      const codes = await detector.detect(scannerVideo.value);
      if (codes.length > 0) {
        await recordQrScan(codes[0].rawValue);
      }
    }, 900);
  } catch (error) {
    scannerMessage.value = error instanceof Error ? error.message : 'Unable to open camera.';
  }
}

function stopQrScanner() {
  if (scannerTimer) {
    window.clearInterval(scannerTimer);
    scannerTimer = null;
  }

  if (scannerStream) {
    scannerStream.getTracks().forEach((track) => track.stop());
    scannerStream = null;
  }

  scannerActive.value = false;
  if (scannerVideo.value) {
    scannerVideo.value.srcObject = null;
  }
}

async function recordQrScan(rawValue, options = {}) {
  const studentNo = parseQrPayload(rawValue);
  const now = Date.now();

  if (studentNo === lastScan.value && now - lastScan.time < 3500) {
    return;
  }

  lastScan = { value: studentNo, time: now };
  const student = students.value.find((item) => item.studentNo === studentNo);

  if (!student) {
    scannerMessage.value = `Student ID ${studentNo || '(blank)'} was not found.`;
    notify('Student not found');
    return;
  }

  try {
    const response = await fetch('/api/attendance/scan', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        studentNo,
        rfidUid: null,
        eventTitle: scanForm.eventTitle || attendanceForm.event,
        status: scanForm.status || attendanceForm.status,
        location: 'QR scanner',
        remarks: 'Recorded from student ID QR.',
      }),
    });

    if (!response.ok) {
      throw new Error(await response.text());
    }

    await response.json();
  } catch (error) {
    scannerMessage.value = 'Saved locally. Backend scan save failed.';
  }

  addAttendanceLocal(scanForm.eventTitle || attendanceForm.event, student.id, scanForm.status || attendanceForm.status);
  manualQr.value = '';
    scannerMessage.value = `${student.name} recorded for ${scanForm.eventTitle}.`;

  if (!options.silent) {
    notify(`${student.name} attendance recorded`);
  }
}

async function recordRfidScan(rawValue) {
  const rfidUid = normalizeRfid(rawValue);
  const now = Date.now();

  if (rfidUid === lastScan.value && now - lastScan.time < 3500) {
    return;
  }

  lastScan = { value: rfidUid, time: now };

  const student = students.value.find((item) => normalizeRfid(item.rfidUid) === rfidUid);
  if (!student) {
    scannerMessage.value = `RFID ${rfidUid || '(blank)'} is not mapped to a student.`;
    notify('RFID not mapped');
    return;
  }

  try {
    const response = await fetch('/api/attendance/scan', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        studentNo: null,
        rfidUid,
        eventTitle: scanForm.eventTitle || attendanceForm.event,
        status: scanForm.status || attendanceForm.status,
        location: 'RFID reader',
        remarks: 'Recorded from RFID card.',
      }),
    });

    if (!response.ok) {
      throw new Error(await response.text());
    }

    await response.json();
  } catch {
    scannerMessage.value = 'Saved locally. Backend RFID save failed.';
  }

  addAttendanceLocal(scanForm.eventTitle || attendanceForm.event, student.id, scanForm.status || attendanceForm.status);
  manualRfid.value = '';
  scannerMessage.value = `${student.name} recorded by RFID for ${scanForm.eventTitle}.`;
  notify(`${student.name} RFID attendance recorded`);
}

function removeAttendance(recordId) {
  attendanceRecords.value = attendanceRecords.value.filter((record) => record.id !== recordId);
  logActivity('Attendance', 'Attendance removed', 'Attendance record deleted');
  notify('Attendance deleted');
}

function addDisbursement() {
  disbursements.value.unshift({
    id: nextId(disbursements.value),
    description: disbursementForm.description,
    amount: Number(disbursementForm.amount || 0),
    month: currentMonth(),
  });
  logActivity('Expense', `${disbursementForm.description} recorded`, money(disbursementForm.amount));
  disbursementForm.description = 'Department expense';
  disbursementForm.amount = 100;
  notify('Expense saved');
}

function removeDisbursement(disbursementId) {
  disbursements.value = disbursements.value.filter((item) => item.id !== disbursementId);
  logActivity('Expense', 'Expense removed', 'Disbursement record deleted');
  notify('Expense deleted');
}

function primaryAction() {
  if (activeView.value === 'dashboard') {
    setView('collections');
  } else if (activeView.value === 'students') {
    resetStudentForm();
    notify('Student form is ready');
  } else if (activeView.value === 'reports') {
    windowPrint();
  }
}

function exportCsv() {
  const rows = [
    ['Type', 'Reference', 'Student', 'Category', 'Amount', 'Status'],
    ...collections.value.map((item) => ['Collection', item.receipt, studentName(item.studentId), item.category, item.amount, 'Recorded']),
    ...fines.value.map((item) => ['Fine', `FINE-${item.id}`, studentName(item.studentId), item.category, item.amount, item.status]),
    ...attendanceRecords.value.map((item) => ['Attendance', item.event, studentName(item.studentId), item.status, '', 'Recorded']),
  ];
  const csv = rows.map((row) => row.map((cell) => `"${String(cell).replaceAll('"', '""')}"`).join(',')).join('\n');
  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' });
  const url = URL.createObjectURL(blob);
  const link = document.createElement('a');
  link.href = url;
  link.download = 'kier-records-export.csv';
  link.click();
  URL.revokeObjectURL(url);
  notify('CSV exported');
}

function windowPrint() {
  window.print();
}

function saveState() {
  localStorage.setItem(
    storageKey,
    JSON.stringify({
      students: students.value,
      collections: collections.value,
      fines: fines.value,
      attendanceRecords: attendanceRecords.value,
      disbursements: disbursements.value,
      activity: activity.value,
    }),
  );
}

function loadState() {
  const stored = localStorage.getItem(storageKey);
  if (!stored) {
    return;
  }

  try {
    const parsed = JSON.parse(stored);
    students.value = (parsed.students || copy(demoData.students)).map((student) => ({
      ...student,
      rfidUid: student.rfidUid || '',
    }));
    collections.value = parsed.collections || copy(demoData.collections);
    fines.value = parsed.fines || copy(demoData.fines);
    attendanceRecords.value = parsed.attendanceRecords || copy(demoData.attendanceRecords);
    disbursements.value = parsed.disbursements || copy(demoData.disbursements);
    activity.value = parsed.activity || copy(demoData.activity);
  } catch {
    resetDemoData();
  }
}

function resetDemoData() {
  students.value = copy(demoData.students);
  collections.value = copy(demoData.collections);
  fines.value = copy(demoData.fines);
  attendanceRecords.value = copy(demoData.attendanceRecords);
  disbursements.value = copy(demoData.disbursements);
  activity.value = copy(demoData.activity);
  selectedStudentId.value = null;
  resetStudentForm();
  notify('Demo data reset');
}
</script>

<style>
* {
  box-sizing: border-box;
}

body {
  margin: 0;
  background: #eef2f3;
  color: #142027;
}

button,
input,
select {
  font: inherit;
}

button {
  cursor: pointer;
}

#app {
  min-height: 100vh;
  font-family: Inter, Segoe UI, Arial, sans-serif;
}

.workspace {
  display: grid;
  grid-template-columns: 268px minmax(0, 1fr);
  min-height: 100vh;
}

.sidebar {
  position: sticky;
  top: 0;
  display: flex;
  flex-direction: column;
  gap: 22px;
  height: 100vh;
  padding: 22px;
  background: #142027;
  color: #ffffff;
}

.brand,
.api-card {
  display: flex;
  gap: 12px;
  align-items: center;
}

.brand-mark {
  display: grid;
  width: 44px;
  height: 44px;
  place-items: center;
  border-radius: 8px;
  background: #f59e0b;
  color: #142027;
  font-weight: 900;
}

.brand small,
.api-card small,
.sidebar-summary small {
  display: block;
  margin-top: 4px;
  color: #a9b7be;
}

.nav-list {
  display: grid;
  gap: 8px;
}

.nav-list button {
  display: flex;
  gap: 10px;
  align-items: center;
  min-height: 42px;
  padding: 0 12px;
  border: 0;
  border-radius: 8px;
  background: transparent;
  color: #d8e0e4;
  text-align: left;
}

.nav-list button span {
  display: grid;
  width: 26px;
  height: 26px;
  place-items: center;
  border-radius: 6px;
  background: #24343d;
  font-size: 0.72rem;
  font-weight: 900;
}

.nav-list button.active,
.nav-list button:hover {
  background: #23343d;
  color: #ffffff;
}

.sidebar-summary,
.api-card {
  padding: 15px;
  border: 1px solid #31444f;
  border-radius: 8px;
  background: #1a2a32;
}

.sidebar-summary span {
  color: #a9b7be;
  font-size: 0.78rem;
  font-weight: 800;
  text-transform: uppercase;
}

.sidebar-summary strong {
  display: block;
  margin-top: 8px;
  font-size: 1.4rem;
}

.api-card {
  margin-top: auto;
}

.status-dot {
  width: 12px;
  height: 12px;
  flex: 0 0 12px;
  border-radius: 999px;
  background: #f59e0b;
}

.status-dot.online {
  background: #16a34a;
}

.content {
  position: relative;
  min-width: 0;
  padding: 28px;
}

.topbar {
  display: flex;
  gap: 24px;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 18px;
}

.eyebrow {
  margin: 0 0 6px;
  color: #60717a;
  font-size: 0.78rem;
  font-weight: 800;
  text-transform: uppercase;
}

h1,
h2,
p {
  margin-top: 0;
}

h1 {
  margin-bottom: 0;
  font-size: 2rem;
  line-height: 1.15;
  letter-spacing: 0;
}

h2 {
  margin-bottom: 0;
  font-size: 1rem;
}

.toolbar,
.button-row {
  display: flex;
  gap: 10px;
  align-items: end;
}

.search {
  display: grid;
  gap: 6px;
  color: #60717a;
  font-size: 0.78rem;
  font-weight: 700;
}

input,
select {
  min-height: 40px;
  min-width: 190px;
  border: 1px solid #ccd6db;
  border-radius: 8px;
  padding: 0 12px;
  background: #ffffff;
  color: #142027;
}

.primary-action,
.secondary-action,
.panel-heading button,
.table-actions button,
.text-action,
.drawer-close {
  min-height: 38px;
  border: 0;
  border-radius: 8px;
  padding: 0 13px;
  font-weight: 800;
}

.primary-action {
  background: #0f766e;
  color: #ffffff;
}

.secondary-action,
.panel-heading button,
.table-actions button,
.drawer-close {
  border: 1px solid #cfd9de;
  background: #ffffff;
  color: #20313a;
}

.text-action,
.row-link {
  min-height: auto;
  border: 0;
  padding: 0;
  background: transparent;
  color: #0f766e;
  font-weight: 800;
}

.toast {
  width: fit-content;
  margin: 0 0 16px auto;
  border-radius: 8px;
  padding: 10px 14px;
  background: #142027;
  color: #ffffff;
  font-weight: 700;
}

.view-stack {
  display: grid;
  gap: 20px;
}

.stat-grid {
  display: grid;
  grid-template-columns: repeat(4, minmax(0, 1fr));
  gap: 16px;
}

.stat-card,
.panel,
.student-drawer {
  border: 1px solid #d7e0e4;
  border-radius: 8px;
  background: #ffffff;
}

.stat-card {
  display: grid;
  gap: 12px;
  min-height: 142px;
  padding: 20px;
}

.stat-card span,
.stat-card small,
.panel-heading span {
  color: #60717a;
}

.stat-card strong {
  font-size: 1.65rem;
}

.split-grid,
.data-layout {
  display: grid;
  grid-template-columns: minmax(0, 1fr) minmax(0, 1fr);
  gap: 20px;
}

.data-layout {
  grid-template-columns: 320px minmax(0, 1fr);
}

.qr-scanner-grid {
  display: grid;
  grid-template-columns: 320px minmax(0, 1fr);
  gap: 20px;
}

.panel {
  min-width: 0;
  overflow: hidden;
}

.panel-heading {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  padding: 18px 20px;
  border-bottom: 1px solid #e2e8eb;
}

.panel-heading.flat {
  padding: 0 0 18px;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 14px 20px;
  border-bottom: 1px solid #edf1f3;
  text-align: left;
  vertical-align: middle;
}

th {
  color: #60717a;
  font-size: 0.78rem;
  text-transform: uppercase;
}

tbody tr {
  transition: background 0.15s ease;
}

tbody tr:hover {
  background: #f5faf9;
}

.table-actions {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.chart-list {
  display: grid;
  gap: 16px;
  padding: 20px;
}

.chart-row {
  display: grid;
  grid-template-columns: 52px minmax(0, 1fr) 90px;
  gap: 12px;
  align-items: center;
}

.bar-track {
  position: relative;
  height: 18px;
  overflow: hidden;
  border-radius: 999px;
  background: #e6ecef;
}

.bar {
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
}

.bar.income {
  background: #0f766e;
}

.bar.expense {
  background: rgba(245, 158, 11, 0.82);
}

.activity-list {
  display: grid;
  gap: 12px;
  margin: 0;
  padding: 20px;
  list-style: none;
}

.activity-list li {
  display: grid;
  grid-template-columns: 82px minmax(0, 1fr);
  gap: 12px;
}

.activity-list li > span {
  align-self: start;
  border-radius: 999px;
  padding: 5px 9px;
  background: #eef7f6;
  color: #0f766e;
  font-size: 0.76rem;
  font-weight: 900;
  text-align: center;
}

.activity-list small {
  display: block;
  margin-top: 3px;
  color: #60717a;
}

.form-panel {
  display: grid;
  gap: 16px;
  align-content: start;
  padding: 20px;
}

.form-title {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.form-panel label {
  display: grid;
  gap: 7px;
  color: #60717a;
  font-size: 0.82rem;
  font-weight: 800;
}

.form-panel input,
.form-panel select {
  width: 100%;
  min-width: 0;
}

.scanner-body {
  display: grid;
  grid-template-columns: minmax(260px, 420px) minmax(240px, 1fr);
  gap: 18px;
  padding: 20px;
}

.scanner-video {
  width: 100%;
  min-height: 280px;
  border: 1px solid #ccd6db;
  border-radius: 8px;
  background: #142027;
  object-fit: cover;
}

.scanner-controls {
  display: grid;
  gap: 14px;
  align-content: start;
}

.scanner-controls label {
  display: grid;
  gap: 7px;
  color: #60717a;
  font-size: 0.82rem;
  font-weight: 800;
}

.scanner-controls input,
.scanner-controls select {
  width: 100%;
}

.scanner-message {
  grid-column: 1 / -1;
  margin: 0;
  border-radius: 8px;
  padding: 12px;
  background: #f4f8f9;
  color: #394a53;
}

.badge {
  display: inline-flex;
  min-width: 70px;
  justify-content: center;
  border-radius: 999px;
  padding: 5px 10px;
  background: #fee2e2;
  color: #991b1b;
  font-size: 0.78rem;
  font-weight: 800;
}

.badge.paid {
  background: #dcfce7;
  color: #166534;
}

.badge.neutral {
  background: #e0f2fe;
  color: #075985;
}

.report-panel {
  padding: 20px;
}

.report-list {
  display: grid;
  gap: 12px;
  margin: 0 0 20px;
}

.report-list div,
.mini-stats span {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
}

.report-list dt {
  color: #60717a;
}

.report-list dd {
  margin: 0;
  font-weight: 900;
}

.student-drawer {
  position: fixed;
  right: 24px;
  bottom: 24px;
  width: min(360px, calc(100vw - 48px));
  padding: 20px;
  box-shadow: 0 18px 50px rgba(20, 32, 39, 0.18);
}

.drawer-close {
  float: right;
}

.student-drawer p {
  color: #60717a;
}

.qr-card {
  display: grid;
  justify-items: center;
  gap: 10px;
  margin: 18px 0;
  border: 1px solid #d7e0e4;
  border-radius: 8px;
  padding: 14px;
  background: #ffffff;
}

.qr-card img {
  width: 180px;
  height: 180px;
}

.qr-card figcaption {
  color: #60717a;
  font-size: 0.82rem;
  font-weight: 800;
}

.mini-stats {
  display: grid;
  gap: 10px;
  margin-top: 18px;
}

.mini-stats span {
  border-radius: 8px;
  padding: 12px;
  background: #f4f8f9;
  color: #60717a;
}

.mini-stats strong {
  color: #142027;
}

@media (max-width: 1040px) {
  .workspace,
  .topbar,
  .split-grid,
  .data-layout,
  .qr-scanner-grid,
  .scanner-body,
  .stat-grid {
    grid-template-columns: 1fr;
  }

  .sidebar {
    position: static;
    height: auto;
  }

  .toolbar {
    width: 100%;
    flex-wrap: wrap;
  }

  .search,
  .search input,
  .primary-action,
  .secondary-action {
    width: 100%;
  }
}

@media print {
  .sidebar,
  .toolbar,
  .toast,
  .student-drawer {
    display: none;
  }

  .workspace {
    display: block;
  }

  .content {
    padding: 0;
  }
}
</style>
