# Introduction to Artificial Intelligence - Slides

This folder contains a Slidev presentation introducing Artificial Intelligence concepts for an AI course.

## About Slidev

[Slidev](https://sli.dev) is a presentation tool for developers. It allows you to write slides in Markdown and provides powerful features like code highlighting, interactive components, and more.

## Prerequisites

- Node.js (version 14 or higher)
- npm or yarn package manager

## Installation

Install the dependencies:

```bash
npm install
```

## Usage

### Development Mode

Start the development server to view and edit slides:

```bash
npm run dev
```

This will start a local server (usually at http://localhost:3030) where you can view your slides. The slides will auto-reload when you make changes to `slides.md`.

### Build for Production

Build static HTML files for hosting:

```bash
npm run build
```

The built files will be in the `dist` folder.

### Export to PDF

Export slides to PDF:

```bash
npm run export
```

## Slide Content

The presentation covers:

1. **What is AI?** - Fundamental concepts and definitions
2. **History of AI** - Key milestones and evolution
3. **Types of AI** - Narrow AI, General AI, and Super AI
4. **Core Technologies** - ML, DL, NLP, Computer Vision
5. **Machine Learning** - Different learning paradigms
6. **Neural Networks** - Basic structure and components
7. **Applications** - Real-world AI use cases
8. **Tools & Frameworks** - Popular AI development tools
9. **Ethics & Challenges** - Responsible AI development
10. **Future of AI** - Short, medium, and long-term predictions
11. **Getting Started** - Learning resources and prerequisites

## Customization

Edit the `slides.md` file to customize the content. Slidev uses Markdown with special syntax for slide separation and features:

- Use `---` to separate slides
- Use `<v-click>` for click animations
- Use layouts like `layout: two-cols` for different slide structures
- Add code blocks with syntax highlighting

## Learn More

- [Slidev Documentation](https://sli.dev)
- [Slidev GitHub](https://github.com/slidevjs/slidev)
- [Markdown Guide](https://www.markdownguide.org/)
